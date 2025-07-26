import {DropdownModel} from '../models/common/common.model';

export enum Difficulty {
  Easy,
  Medium,
  Hard ,
}

export const DifficultyDropdown: DropdownModel[] = [
  { name: 'Easy', id: Difficulty.Easy },
  { name: 'Medium', id: Difficulty.Medium },
  { name: 'Hard', id: Difficulty.Hard }
]
