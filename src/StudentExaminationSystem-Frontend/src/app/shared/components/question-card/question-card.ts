import { Component, Input, Output, EventEmitter, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RadioButtonModule } from 'primeng/radiobutton';
import { FormsModule } from '@angular/forms';
import { QuestionListModel } from '../../../core/models/question.mode';

interface DifficultyOption {
  label: string;
  value: number;
}

@Component({
  selector: 'app-question-card',
  templateUrl: './question-card.html',
  imports: [
    CommonModule,
    RadioButtonModule,
    FormsModule
  ],
  styleUrls: ['./question-card.scss']
})
export class QuestionCardComponent {
  @Input() question = signal<QuestionListModel | null>(null);
  @Input() viewMode = signal<boolean>(true);
  @Input() showMetadata = signal<boolean>(true);
  @Input() selectedChoiceId = signal<number | null>(null);

  @Output() choiceSelected = new EventEmitter<number>();

  onChoiceChange(choiceId: number) {
    if (!this.viewMode()) {
      this.selectedChoiceId.set(choiceId);
      this.choiceSelected.emit(choiceId);
    }
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

  isCorrectChoice(choiceId: number): boolean {
    const question = this.question();
    if (!question) return false;
    const choice = question.choices.find(c => c.id === choiceId);
    return choice?.isCorrect || false;
  }
}
