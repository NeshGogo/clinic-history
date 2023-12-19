import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-record',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div>
      <p><span class="font-bold">Doctor: </span> {{doctorName}}</p>
      <p><span class="font-bold">Speciality: </span> {{doctorSpeciality}} </p>
      <small>{{date | date }}</small>
      <p class="p-1 border border-gray-200 rounded whitespace-pre">{{ diagnosis }}</p>
    </div>
  `,
})
export class RecordComponent {
  @Input() doctorName!: string;
  @Input() doctorSpeciality!: string;
  @Input() diagnosis!: string;
  @Input() date!: Date;
}
