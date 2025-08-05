import {DropdownModel} from '../models/common/common.model';

export enum Difficulty {
  Easy = 1,
  Medium = 2,
  Hard = 3
}

export const DifficultyDropdown: DropdownModel[] = [
  { name: 'Easy', id: Difficulty.Easy },
  { name: 'Medium', id: Difficulty.Medium },
  { name: 'Hard', id: Difficulty.Hard }
]
