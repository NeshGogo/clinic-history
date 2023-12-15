import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DrawerComponent } from '../shared/components/drawer/drawer.component';
import { ClinicRecordWithPatientFormComponent } from './components/clinic-record-form/clinic-record-with-patient-form.component';
import { PatientListComponent } from './components/patient-list/patient-list.component';
import { PatientService } from '../core/services/patient.service';
import { Patient } from '../core/models/patient';
import { Router } from '@angular/router';
import { PaginationComponent } from '../shared/components/pagination/pagination.component';
import { Pagination } from '../core/models/pagination';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, DrawerComponent, ClinicRecordWithPatientFormComponent, PatientListComponent,  PaginationComponent,],
  template: `
    <main class="p-2">
      <h1 class="mb-4 text-lg font-extrabold leading-none tracking-tight text-gray-700 md:text-xl lg:text-2xl text-center">
        {{ title }}
      </h1>
      <div class="flex justify-center w-full">
        <div
          class="w-full max-w-3xl p-2 rounded-md shadow-sm bg-white border-2 border-gray-400 flex justify-between items-center flex-wrap  border-dashed"
        >
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
      <div class="mt-4 ">
        <app-patient-list (itemClick)="openPatient($event)" [patients]="patients"></app-patient-list>
      </div>
      <div class="mt-4">
        <app-pagination
          [currentPage]="pagination().page"
          [itemsPerPage]="pagination().size"
          [totalItems]="pagination().total"
          (pageChanged)="onPageChange($event)"
        ></app-pagination>
      </div>
    </main>
    <app-drawer [title]="formTitle" [(open)]="displayForm">
      <app-clinic-record-with-patient-form (OnSave)="onSave()"></app-clinic-record-with-patient-form>
    </app-drawer>
  `,
})
export class HomeComponent implements OnInit {
  title = 'Patients';
  formTitle = 'Add a new patient record';
  displayForm = false;
  patients = signal<Patient[]>([]);
  pagination = signal<Pagination>({
    size: 6,
    page: 1,
    total: 0,
  });

  constructor(private service: PatientService, private router: Router) {
  }

  ngOnInit(): void {
    this.fetchData();
  }

  openForm() {
    this.displayForm = true;
  }

  onSave() {
    this.fetchData();
    this.displayForm = false;
  }

  fetchData() {
    this.service.getAll(this.pagination())
    .subscribe((response) => {
      this.pagination.update((value) => value = {...value, total: parseInt(response.headers.get("total") || "0")});
      this.patients.set(response.body || []);
    });
  }

  openPatient(patient: Patient){
    this.router.navigate(['/history', patient.id]);
  }

  onPageChange(page: number): void {
    this.pagination.update((value) => value = {...value, page})
    this.fetchData();
  }
}
