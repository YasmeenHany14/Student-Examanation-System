import {Component, OnInit, signal, computed, Input} from '@angular/core';
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
export class ExamComponent implements OnInit {
  examForm!: FormGroup;
  currentQuestionIndex = signal<number>(0);
  isLoading = signal<boolean>(true);
  subjectId: number | null = null;
  exam = signal<LoadExamModel | null>(null);

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
    this.route.paramMap.subscribe(params => {
      const subjectId = params.get('subjectId');
      if (subjectId) {
        this.subjectId = +subjectId;
        this.loadExamForSubject(this.subjectId);
      }
    });
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

  private setupFormForExam(exam: LoadExamModel) {
    this.examForm.patchValue({ examId: exam.id });

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

    // Auto-advance to next question after selection (optional)
    setTimeout(() => {
      if (this.currentQuestionIndex() < this.totalQuestions() - 1) {
        this.nextQuestion();
      }
    }, 500);
  }

  getSelectedAnswer(questionIndex: number): number | null {
    const answersArray = this.examForm.get('answers') as FormArray;
    const answer = answersArray.at(questionIndex);
    return answer?.get('selectedChoiceId')?.value || null;
  }

  isQuestionAnswered(index: number): boolean {
    return this.getSelectedAnswer(index) !== null;
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

    // Implementation for exam submission would go here
    console.log('Exam submitted:', this.examForm.value);

    this.messageService.add({
      severity: 'success',
      summary: 'Success',
      detail: 'Exam submitted successfully'
    });
  }

  canGoNext(): boolean {
    return this.currentQuestionIndex() < this.totalQuestions() - 1;
  }

  canGoPrevious(): boolean {
    return this.currentQuestionIndex() > 0;
  }

  getQuestionBoxClass(index: number): string {
    const classes = ['question-box'];

    if (index === this.currentQuestionIndex()) {
      classes.push('current');
    }

    if (this.isQuestionAnswered(index)) {
      classes.push('answered');
    } else {
      classes.push('unanswered');
    }

    return classes.join(' ');
  }

  protected readonly signal = signal;
}
