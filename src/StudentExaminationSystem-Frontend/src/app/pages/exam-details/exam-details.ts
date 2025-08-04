import {Component, computed, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {ExamService} from '../../core/services/exam.service';
import {GetExamHistoryModel} from '../../core/models/exam.model';
import {getFromQueryParams} from '../../shared/utils/nav.utils';
import {ActivatedRoute, Router} from '@angular/router';
import {routes} from '../../core/constants/routs';
import {Spinner} from '../../shared/components/spinner/spinner';
import {QuestionCardComponent} from '../../shared/components/question-card/question-card';
import {QuestionHistoryModel} from '../../core/models/question.model';
import {Carousel} from 'primeng/carousel';
import {Button} from 'primeng/button';
import {Tag} from 'primeng/tag';
import {NgIf} from '@angular/common';
import {NoDataToShowComponent} from '../../shared/components/no-data-to-show/no-data-to-show';
import {ErrorMessageComponent} from '../../shared/components/error-message/error-message';

@Component({
  selector: 'app-exam-details',
  imports: [
    Spinner,
    QuestionCardComponent,
    Carousel,
    Tag,
    NgIf,
    NoDataToShowComponent,
    ErrorMessageComponent
  ],
  templateUrl: './exam-details.html',
  styleUrl: './exam-details.scss'
})
export class ExamDetails implements OnInit {
  examModel = signal<GetExamHistoryModel | null>(null);
  isLoading = signal<boolean>(true);
  isError = signal<boolean>(false);

  private examService = inject(ExamService);
  private destroyRef = inject(DestroyRef);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  ngOnInit() {
    // get from query params
    const examId = getFromQueryParams(this.route, 'examId');
    if (examId) {
      const examIdNumber = Number(examId);
      if (isNaN(examIdNumber)) {
        this.router.navigate([routes.home]);
        return;
      }
      this.loadExamDetails(Number(examId));
    } else {
      this.router.navigate([routes.home]);
    }
  }

  loadExamDetails(examId: number) {
    this.isLoading.set(true);
    this.isError.set(false);
    const subscription = this.examService.getById<GetExamHistoryModel>(examId).subscribe({
      next: data => {
        this.examModel.set(data);
        this.isLoading.set(false);
        this.isError.set(false);
      },
      error: err => {
        this.isLoading.set(false);
        this.isError.set(true);
      }
    })
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    })
  }

  getCorrectAnswersCount() {
    let count = 0;
    if (!this.examModel()?.questions) {
      return count;
    }
    this.examModel()?.questions.forEach((question => {
      question.choices.forEach(choice => (choice.isCorrect && choice.isSelected) && count++);
    }))
    return count;
  }

  getIncorrectAnswersCount() {
    let count = 0;
    if (!this.examModel()?.questions) {
      return count;
    }
    this.examModel()?.questions.forEach((question => {
      let isCorrect = question.choices.some(choice => choice.isCorrect && choice.isSelected);
      count += (isCorrect ? 0 : 1);
    }))
    return count;
  }

  setTag(question: QuestionHistoryModel): [string, string] {
    if (!question.choices.some(c => c.isSelected)) {
      return ['Not Answered', 'danger'];
    }
    return ['Answered', 'info'];
  }

  getSelectedChoiceId(question: QuestionHistoryModel) {
    return question.choices.find(c => c.isSelected)?.id || null;
  }
}
