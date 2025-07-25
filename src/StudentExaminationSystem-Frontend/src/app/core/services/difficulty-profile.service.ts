import {Injectable} from '@angular/core';
import {routes} from '../constants/routs';
import {DropdownBaseService} from './dropdown-base.service';

@Injectable({
  providedIn: 'root'
})

export class DifficultyProfileService extends DropdownBaseService{
    protected override dropdownRoute: string;
    protected override route: string;

    constructor() {
      super();
      this.route = routes.difficultyProfile;
      this.dropdownRoute = routes.difficultyProfileDropdown;
    }

}
