import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Patient } from 'src/app/core/models/patient';

@Component({
  selector: 'app-patient-card',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="flex flex-col bg-white rounded-lg shadow-md p-4 cursor-pointer">
      <div class="flex items-center">
        <div class=" w-10 h-16 rounded-full ">
          <span class="material-symbols-outlined"> person </span>
        </div>
        <div>
          <h2 class="text-xl font-bold text-gray-800">{{ patient?.fullName }}</h2>
          <p class="text-sm text-gray-500">Patient ID: {{ patient?.identification }}</p>
        </div>
      </div>
      <hr class="my-2 border-gray-200" />
      <ul class="flex flex-col space-y-2">
        <li class="flex items-center text-gray-600">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-2 text-gray-400" viewBox="0 0 20 20" fill="currentColor">
            <path d="M2 10a8 8 0 011.5 6.5h4.a8 8 0 017.77 8.77A8 8 0 0120 2.25V4a8 8 0 01-1.5-6.5H14a8 8 0 01-7.77-8.77A8 8 0 012 2.25v1.5z" />
            <path d="M10 14a4 4 0 100-8 4 4 0 000 8z" />
          </svg>
          Sex: {{ patient?.sex }}
        </li>
        <li class="flex items-center text-gray-600">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-2 text-gray-400" viewBox="0 0 20 20" fill="currentColor">
            <path d="M2 10a8 8 0 011.5 6.5h4.a8 8 0 017.77 8.77A8 8 0 0120 2.25V4a8 8 0 01-1.5-6.5H14a8 8 0 01-7.77-8.77A8 8 0 012 2.25v1.5z" />
            <path d="M10 14a4 4 0 100-8 4 4 0 000 8z" />
          </svg>
          Created: {{ patient?.recordCreated | date }}
        </li>
      </ul>
    </div>
  `,
})
export class PatientCardComponent {
  @Input() patient: Patient | null = null;
}
