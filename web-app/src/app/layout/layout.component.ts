import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from '../shared/components/nav/nav.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, NavComponent],
  template: `
    <app-nav [showOpenBtn]="false"></app-nav>
    <main class="p-2">
      <router-outlet></router-outlet>
    </main>
  `,
})
export class LayoutComponent {

}
