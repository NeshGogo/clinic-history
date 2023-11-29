import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavComponent } from '../shared/components/nav/nav.component';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, NavComponent],
  template: `
    <app-nav></app-nav>
    <main class="p-2">
      <h1 class="mb-4 text-lg font-extrabold leading-none tracking-tight text-gray-700 md:text-xl lg:text-2xl text-center">
        {{ title }}
      </h1>
      <div class="fixed z-50 bg-white flex flex-col justify-center items-center w-full">
        <div class="w-full max-w-3xl px-4 py-2 rounded-lg shadow-sm bg-white text-right border border-gray-100">
          <p class="font-bold text-sky-600 text-center">Are you taking on a new patient?</p>
          <button class="px-4 py-2 rounded-lg shadow-lg text-blue-700 bg-white">Add New</button>
        </div>
      </div>
    </main>
  `,
})
export class HomeComponent {
  title = 'Patients';
  constructor(titleService: Title) {
    titleService.setTitle('NC | Home');
  }
}
