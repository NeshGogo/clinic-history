import { Component, Input, signal } from '@angular/core';
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
      <div class="mb-2">
        <app-patient-card [patient]="patient"></app-patient-card>
      </div>
      }
    </div>
  `,
})
export class PatientListComponent {
  @Input() patients = signal<Patient[]>([]);
}
