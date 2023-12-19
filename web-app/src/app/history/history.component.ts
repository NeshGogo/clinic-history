import { Component, Input, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PatientService } from '../core/services/patient.service';
import { Patient } from '../core/models/patient';
import { RecordListComponent } from './components/record-list/record-list.component';
import { ClinicRecord } from '../core/models/clinicRecord';
import { ClinicRecordService } from '../core/services/clinic-record.service';

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [CommonModule, RecordListComponent],
  template: `
    <h1 class="mb-4 text-lg font-extrabold leading-none tracking-tight text-gray-700 md:text-xl lg:text-2xl text-center">
      {{ patient()?.fullName }}'s Clinic Record
    </h1>
    <section class="my-0 mx-auto container">
      <div class="text-right">
        <button
          type="button"
          (click)="openForm()"
          class="leading-none  text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-4 focus:ring-green-300 font-medium rounded-full text-lg px-1.5 py-1 text-center mr-2 mb-2"
        >
          <span class="material-symbols-outlined"> add </span>
        </button>
      </div>

      <hr class="mb-2 mt-10" />
      <div class="p-4">
        <app-record-list [records]="records"></app-record-list>
      </div>
    </section>
  `,
})
export class HistoryComponent implements OnInit {
  @Input() id!: string;
  patient = signal<Patient | null>(null);
  records = signal<ClinicRecord[]>([]);

  constructor(private service: PatientService, private recordService: ClinicRecordService) {}

  ngOnInit(): void {
    this.fetchPatient();
    this.fetchRecords();
  }

  fetchPatient() {
    this.service.get(this.id).subscribe((patient) => this.patient.set(patient));
  }

  fetchRecords() {
    this.recordService.getAll(this.id).subscribe((records) => this.records.set(records));
  }

  openForm() {}
}
