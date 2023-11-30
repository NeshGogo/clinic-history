import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavComponent } from '../shared/components/nav/nav.component';
import { Title } from '@angular/platform-browser';
import { DrawerComponent } from '../shared/components/drawer/drawer.component';
import { ClinicRecordWithPatientFormComponent } from './components/clinic-record-form/clinic-record-with-patient-form.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, NavComponent, DrawerComponent, ClinicRecordWithPatientFormComponent],
  template: `
    <app-nav [showOpenBtn]="false"></app-nav>
    <main class="p-2">
      <h1 class="mb-4 text-lg font-extrabold leading-none tracking-tight text-gray-700 md:text-xl lg:text-2xl text-center">
        {{ title }}
      </h1>
      <div class="flex justify-center w-full">
        <div class="w-full max-w-3xl p-2 rounded-md shadow-sm bg-white border border-gray-100 flex justify-between items-center flex-wrap">
          <p class=" text-gray-900">Are you taking on a new patient?</p>
          <button
            type="button"
            (click)="openForm()"
            class="text-white bg-blue-500 hover:bg-blue-600 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-3 py-2 text-center inline-flex items-center "
          >
          Add New
          </button>
        </div>
      </div>
    </main>
    <app-drawer [title]="formTitle" [(open)]="displayForm">
      <app-clinic-record-with-patient-form></app-clinic-record-with-patient-form>
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
