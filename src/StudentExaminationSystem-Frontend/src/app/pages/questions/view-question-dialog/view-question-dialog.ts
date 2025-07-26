import { Component, Input, Output, EventEmitter, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { QuestionListModel } from '../../../core/models/question.mode';

interface DifficultyOption {
  label: string;
  value: number;
}

@Component({
  selector: 'app-view-question-dialog',
  templateUrl: './view-question-dialog.html',
  imports: [
    CommonModule,
    DialogModule,
  ],
  styleUrls: ['./view-question-dialog.scss']
})
export class ViewQuestionDialog {
  @Input() visible = signal(false);
  @Input() question = signal<QuestionListModel | null>(null);
  @Input() mode = signal<'AdminView' | 'History' | 'Solve'>("AdminView");

  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() hide = new EventEmitter<void>();

  onHide() {
    this.visible.set(false);
    this.visibleChange.emit(false);
    this.hide.emit();
  }

  getDifficultyName(difficultyId: number): string {
    const difficultyOptions: DifficultyOption[] = [
      { label: 'Easy', value: 1 },
      { label: 'Medium', value: 2 },
      { label: 'Hard', value: 3 }
    ];
    const option = difficultyOptions.find(d => d.value === difficultyId);
    return option ? option.label : 'Unknown';
  }
}
