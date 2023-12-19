import { Component, Input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClinicRecord } from 'src/app/core/models/clinicRecord';
import { RecordComponent } from '../record/record.component';

@Component({
  selector: 'app-record-list',
  standalone: true,
  imports: [CommonModule, RecordComponent],
  template: `
    <div class="border-l border-blue-400">
      @for (record of records(); track $index) {
      <div class="flex items-center">
        <div class="w-3 h-1 border-t border-blue-400"></div>
        <div class="border-l border-blue-400 pl-2 my-4">
          <app-record
            [doctorName]="record.doctor?.fullName || ''"
            [doctorSpeciality]="record.doctor?.speciality || ''"
            [date]="record.recordCreated"
            [diagnosis]="record.diagnosis"
          ></app-record>
        </div>
      </div>
      }
    </div>
  `,
})
export class RecordListComponent {
  @Input() records = signal<ClinicRecord[]>([]);
}
