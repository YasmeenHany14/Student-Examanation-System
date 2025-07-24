import { Component } from '@angular/core';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

@Component({
  selector: 'app-spinner',
  standalone: true,
  imports: [ProgressSpinnerModule],
  template: `<p-progressSpinner class="mb-6" strokeWidth="3" fill="#eeeeee" animationDuration="1s"/>`,
  styles:`
    p-progressSpinner {
      display: flex;
      justify-content: center;
      align-items: center;
    }
  `
})
export class Spinner {

}
