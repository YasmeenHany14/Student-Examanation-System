import {Component, OnInit, signal, computed, Input, OnDestroy} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ProgressBarModule } from 'primeng/progressbar';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { ExamService } from '../../core/services/exam.service';
import { LoadExamModel } from '../../core/models/exam.model';
import { QuestionCardComponent } from '../../shared/components/question-card/question-card';

@Component({
  selector: 'app-exam',
  templateUrl: './exam.html',
  styleUrls: ['./exam.scss'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ButtonModule,
    CardModule,
    ProgressBarModule,
    ToastModule,
    QuestionCardComponent
  ],
  providers: [MessageService]
})
export class ExamComponent implements OnInit, OnDestroy {
  examForm!: FormGroup;
  currentQuestionIndex = signal<number>(0);
  isLoading = signal<boolean>(true);
  examId: number | null = null;
  exam = signal<LoadExamModel | null>(null);

  // Timer properties
  timeRemaining = signal<number>(0); // in seconds
  isExamSubmitted = false;
  isExamExpired = false;

  currentQuestion = computed(() => {
    const examData = this.exam();
    const index = this.currentQuestionIndex();
    return examData?.questions[index] || null;
  });

  totalQuestions = computed(() => {
    return this.exam()?.questions.length || 0;
  });

  progressPercentage = computed(() => {
    const total = this.totalQuestions();
    if (total === 0) return 0;
    return ((this.currentQuestionIndex() + 1) / total) * 100;
  });

  // Format time remaining as MM:SS
  formattedTimeRemaining = computed(() => {
    const timeInSeconds = this.timeRemaining();
    const minutes = Math.floor(timeInSeconds / 60);
    const seconds = timeInSeconds % 60;
    return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
  });

  isTimeCritical = computed(() => {
    return this.timeRemaining() < 300; // 5 minutes
  });

  answeredQuestions = computed(() => {
    const answers = this.examForm?.get('answers')?.value || [];
    return answers.filter((answer: any) => answer.selectedChoiceId !== null).length;
  });

  constructor(
    private fb: FormBuilder,
    private examService: ExamService,
    private route: ActivatedRoute,
    private router: Router,
    private messageService: MessageService
  ) {
    this.initializeForm();
  }

  ngOnInit() {
    const navigation = this.router.getCurrentNavigation();
    const examData = navigation?.extras.state?.['examData'] as LoadExamModel;

    if (examData) {
      this.setupFormForExam(examData);
    } else {
      // Fallback: try to get exam ID from route params
      this.route.paramMap.subscribe(params => {
        const examId = params.get('id');
        if (examId) {
          this.examId = +examId;
          this.loadExamForSubject(this.examId);
        } else {
          this.router.navigate(['/home/take-exam']);
        }
      });
    }
  }

  ngOnDestroy() {
    // this.clearTimer();
  }

  private initializeForm() {
    this.examForm = this.fb.group({
      examId: [null],
      answers: this.fb.array([])
    });
  }

  private loadExamForSubject(subjectId: number) {
    this.isLoading.set(true);
    this.examService.getExamForSubject(subjectId).subscribe({
      next: (exam) => {
        this.exam.set(exam);
        this.setupFormForExam(exam);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading exam:', error);
        this.isLoading.set(false);
      }
    });
  }

  private startTimer() {
    const examEndTime = this.exam()?.examEndTime;
    const endTime = new Date(examEndTime!).getTime();
    const interval = setInterval(() => {
      const now = new Date().getTime();
      const timeLeft = Math.max(0, Math.floor((endTime - now) / 1000));
      this.timeRemaining.set(timeLeft);

      if (timeLeft <= 0) {
        clearInterval(interval);
        this.isExamExpired = true;
        this.submitExam();
      }
    }, 1000);
  }

  submitExam() {
    if (!this.examForm.valid) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Warning',
        detail: 'Please answer all questions before submitting'
      });
      return;
    }
    this.fromFormToModel();
    this.examService.submitExam(this.exam()!).subscribe({
      next: (exam) => {
        this.isExamSubmitted = true;
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Exam submitted successfully'
        });
        setTimeout(() => {
          this.router.navigate(['/home/exams']);
        }, 2000);
      },
      error: (error) => {
        console.error('Error submitting exam:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to submit exam. Please try again later.'
        });
      }
    })
  }

  private fromFormToModel() {
    this.exam()?.questions.forEach((question) => {
      const answer = this.examForm.get('answers')?.value.find((a: any) => a.questionId === question.id);
      if (answer) {
        question.choices.forEach((choice) => {
          choice.isSelected = answer.selectedChoiceId === choice.id;
        });
      }
    })
    console.log(this.exam());
  }

  private setupFormForExam(exam: LoadExamModel) {
    this.examForm.patchValue({ examId: exam.id });
    this.startTimer();

    const answersArray = this.examForm.get('answers') as FormArray;
    answersArray.clear();

    exam.questions.forEach((question, index) => {
      answersArray.push(this.fb.group({
        questionId: [question.id],
        selectedChoiceId: [null]
      }));
    });
  }

  goToQuestion(index: number) {
    if (index >= 0 && index < this.totalQuestions()) {
      this.currentQuestionIndex.set(index);
    }
  }

  nextQuestion() {
    const nextIndex = this.currentQuestionIndex() + 1;
    if (nextIndex < this.totalQuestions()) {
      this.currentQuestionIndex.set(nextIndex);
    }
  }

  previousQuestion() {
    const prevIndex = this.currentQuestionIndex() - 1;
    if (prevIndex >= 0) {
      this.currentQuestionIndex.set(prevIndex);
    }
  }

  onAnswerSelected(choiceId: number) {
    const currentIndex = this.currentQuestionIndex();
    const answersArray = this.examForm.get('answers') as FormArray;
    const currentAnswer = answersArray.at(currentIndex);

    if (currentAnswer) {
      currentAnswer.patchValue({ selectedChoiceId: choiceId });
    }
  }

  getSelectedAnswer(questionIndex: number): number | null {
    const answersArray = this.examForm.get('answers') as FormArray;
    const answer = answersArray.at(questionIndex);
    return answer?.get('selectedChoiceId')?.value || null;
  }

  isQuestionAnswered(index: number): boolean {
    return this.getSelectedAnswer(index) !== null;
  }

  canGoNext(): boolean {
    return this.currentQuestionIndex() < this.totalQuestions() - 1;
  }

  canGoPrevious(): boolean {
    return this.currentQuestionIndex() > 0;
  }

  protected readonly signal = signal;

  getQuestionButtonClass(index: number): string {
    if (index === this.currentQuestionIndex()) {
      return 'bg-blue-100 border-blue-500 text-blue-700';
    }
    if (this.isQuestionAnswered(index)) {
      return 'bg-green-100 border-green-500 text-green-700';
    }
    return 'bg-gray-100 border-gray-300 text-gray-600 hover:bg-gray-200';
  }
}
