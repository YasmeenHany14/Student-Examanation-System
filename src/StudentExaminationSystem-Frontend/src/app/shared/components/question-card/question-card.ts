import {Component, Input, Output, EventEmitter, signal, computed, input} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RadioButtonModule } from 'primeng/radiobutton';
import { FormsModule } from '@angular/forms';
import {QuestionHistoryModel, QuestionListModel} from '../../../core/models/question.model';
import {DifficultyDropdown} from '../../../core/enums/difficulty';

interface Metadata {
  subjectName: string;
  difficultyName: string;
  isActive: boolean;
}


type QuestionModel = QuestionListModel | QuestionHistoryModel;

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
  question = input<QuestionModel | null>();
  viewMode = input<boolean>(true);
  showMetadata = input<boolean>(true);
  selectedChoiceId = input<number | null>(null);

  @Output() choiceSelected = new EventEmitter<number>();

  onChoiceChange(choiceId: number) {
    if (!this.viewMode()) {
      this.choiceSelected.emit(choiceId);
    }
  }

  getDifficultyName(difficultyId: number): string {
    const difficultyOptions = DifficultyDropdown;
    const option = difficultyOptions.find(d => d.id === difficultyId);
    return option ? option.name : 'Unknown';
  }

  getMetadata = computed((): Metadata => {
    const question = this.question();
    // Default values
    let subjectName = 'Unknown Subject';
    let difficultyName = 'Unknown';
    let isActive = true;

    if (question) {
      if ('subjectName' in question && typeof question.subjectName === 'string') {
        subjectName = question.subjectName;
      }
      if ('difficultyId' in question && typeof question.difficultyId === 'number') {
        difficultyName = this.getDifficultyName(question.difficultyId);
      }
      if ('isActive' in question && typeof question.isActive === 'boolean') {
        isActive = question.isActive;
      }
    }

    return { subjectName, difficultyName, isActive };
  });

  isCorrectChoice(choiceId: number): boolean {
    const question = this.question();
    if (!question) return false;
    const choice = question.choices.find(c => c.id === choiceId);
    return choice?.isCorrect || false;
  }
}
