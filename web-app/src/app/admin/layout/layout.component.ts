import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavComponent } from '../../shared/components/nav/nav.component';
import { SidebarComponent } from '../../shared/components/sidebar/sidebar.component';
import { LinkItem } from '../../core/models/linkItem';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [CommonModule, NavComponent, SidebarComponent, RouterOutlet],
  template: `
    <main class="flex">
      <app-sidebar
        app-sidebar
        [items]="menuItems"
        [(open)]="show"
      ></app-sidebar>
      <div class="w-full">
        <app-nav (showSideBarMenu)="showMenu()"></app-nav>
        <div class="p-2">
          <router-outlet></router-outlet>
        </div>
      </div>
    </main>
  `,
})
export class LayoutComponent {
  show = false;
  menuItems: LinkItem[] = [
    {
      name: 'Doctors',
      path: 'doctors',
      iconUrl:
        '/assets/imgs/doctor-icon.png',
      iconAlt: 'Doctors icon',
    },
    {
      name: 'Specialities',
      path: 'specialities',
      iconUrl: '/assets/imgs/medicine-icon.png',
      iconAlt: 'specialities icon',
    },
  ];

  showMenu() {
    this.show = true;
  }
}
