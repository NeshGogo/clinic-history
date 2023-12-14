import { Component, Input, signal, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Patient } from 'src/app/core/models/patient';
import { PatientCardComponent } from '../patient-card/patient-card.component';

@Component({
  selector: 'app-patient-list',
  standalone: true,
  imports: [CommonModule, PatientCardComponent],
  template: `
    <div class="max-w-7xl border-t-2 p-4 border-gray-200 m-0 mx-auto">
      @for (patient of patients(); track $index) {
      <div class="mb-2" (click)="onClick(patient)">
        <app-patient-card [patient]="patient"></app-patient-card>
      </div>
      }
      @empty {
        <p class="text-center" >No patients have been added yet....</p>
      }
    </div>
  `,
})
export class PatientListComponent {
  @Input() patients = signal<Patient[]>([]);
  @Output() itemClick = new EventEmitter<Patient>();

  onClick(patient: Patient) {
    this.itemClick.emit(patient);
  }
}
