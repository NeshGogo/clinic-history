import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavComponent } from '../shared/components/nav/nav.component';
import { Title } from '@angular/platform-browser';
import { DrawerComponent } from '../shared/components/drawer/drawer.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, NavComponent, DrawerComponent],
  template: `
    <app-nav [showOpenBtn]="false"></app-nav>
    <main class="p-2">
      <h1 class="mb-4 text-lg font-extrabold leading-none tracking-tight text-gray-700 md:text-xl lg:text-2xl text-center">
        {{ title }}
      </h1>
      <div class="bg-white flex flex-col justify-center items-center w-full">
        <div class="w-full max-w-3xl px-4 py-2 rounded-lg shadow-sm bg-white text-right border border-gray-100">
          <p class="font-bold text-sky-600 text-center">Are you taking on a new patient?</p>
          <button class="px-4 py-2 rounded-lg shadow-lg text-blue-700 bg-white" (click)="openForm()">Add New</button>
        </div>
      </div>
    </main>
    <app-drawer
      [title]="formTitle"
      [(open)]="displayForm"
    >

    </app-drawer>
  `,
})
export class HomeComponent {
  title = 'Patients';
  formTitle = 'Add a new patient record';
  displayForm = false;
  constructor(titleService: Title) {
    titleService.setTitle('NC | Home');
  }
  openForm() {
    this.displayForm = true;
  }
}
