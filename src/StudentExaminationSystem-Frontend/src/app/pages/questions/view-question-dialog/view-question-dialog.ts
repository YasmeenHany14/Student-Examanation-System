import { Component, Input, Output, EventEmitter, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { QuestionListModel } from '../../../core/models/question.model';
import { QuestionCardComponent } from '../../../shared/components/question-card/question-card';

@Component({
  selector: 'app-view-question-dialog',
  templateUrl: './view-question-dialog.html',
  imports: [
    CommonModule,
    DialogModule,
    QuestionCardComponent
  ],
  styleUrls: ['./view-question-dialog.scss']
})
export class ViewQuestionDialog {
  @Input() visible = signal(false);
  @Input() question = signal<QuestionListModel | null>(null);

  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() hide = new EventEmitter<void>();

  onHide() {
    this.visible.set(false);
    this.visibleChange.emit(false);
    this.hide.emit();
  }
}
