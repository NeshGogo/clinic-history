import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ClinicRecord, ClinicRecordCreateDto } from 'src/app/core/models/clinicRecord';
import { Doctor } from 'src/app/core/models/doctor';
import { ClinicRecordService } from 'src/app/core/services/clinic-record.service';
import { HttpErrorResponse } from '@angular/common/http';
import swal from 'sweetalert2';

@Component({
  selector: 'app-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <form [formGroup]="form" (ngSubmit)="submit($event)">
      <div class="mb-2">
        <label for="doctorId" class="block mb-2 text-sm font-medium text-gray-900">Doctor</label>
        <select
          id="doctorId"
          formControlName="doctorId"
          class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
        >
          <option [value]="null">Choose a doctor</option>
          @for (doctor of doctors(); track $index) {
          <option [value]="doctor.id">
            {{ doctor.fullName }}
          </option>
          }
        </select>
        @if (form.get('doctorId')?.touched && form.get('doctorId')?.hasError('nullValidator')) {
        <p class="text-red-700">* This field is requiered.</p>
        }
      </div>
      <div class="mb-2">
        <label for="diagnosis" class="block mb-2 text-sm font-medium text-gray-900">Diagnosis</label>
        <textarea
          id="diagnosis"
          rows="14"
          formControlName="diagnosis"
          class="block p-2.5 w-full text-sm text-gray-900 bg-gray-50 rounded-lg border border-gray-300 focus:ring-blue-500 focus:border-blue-500"
          placeholder="Write the diagnosis here..."
        ></textarea>
        @if (form.get('diagnosis')?.touched && form.get('diagnosis')?.hasError('maxLength')) {
        <p class="text-red-700">* The max length is 800 characters.</p>
        }
      </div>
      <button
        type="submit"
        [disabled]="!form.valid"
        [className]="
          !form.valid
            ? 'bg-blue-300 hover:bg-blue-300 cursor-not-allowed text-white w-full focus:outline-none focus:ring-4 focus:ring-blue-300 font-medium rounded-full text-sm px-5 py-2.5 text-center mr-2 mb-2'
            : 'bg-blue-700 hover:bg-blue-800 text-white w-full focus:outline-none focus:ring-4 focus:ring-blue-300 font-medium rounded-full text-sm px-5 py-2.5 text-center mr-2 mb-2'
        "
      >
        Save
      </button>
    </form>
  `,
})
export class FormComponent {
  @Output() onSave: EventEmitter<ClinicRecord> = new EventEmitter();
  @Input() id!: string;
  doctors = signal<Doctor[]>([]);
  form: FormGroup = this.formBuilder.group({
    doctorId: [null, [Validators.required, Validators.maxLength(36), Validators.minLength(36), Validators.nullValidator]],
    diagnosis: ['', [Validators.maxLength(800)]],
  });

  constructor(private formBuilder: FormBuilder, private service: ClinicRecordService) {}

  submit(ev: Event) {
    ev.preventDefault();

    if (!this.form?.valid) {
      console.log('The form is invalid!');
      return;
    }
    const body: ClinicRecordCreateDto = {
      patientId: this.id,
      doctorId: this.form.value.doctorId,
      diagnosis: this.form.value.diagnosis,
    };
    this.service.add(this.id, body).subscribe({
      next: (record) => {
        this.form.reset();
        this.showAlert('Successed', 'success', 'Record added successful!', true);
        this.onSave.emit(record);
      },
      error: (error: HttpErrorResponse) => {
        if (error.status == 400) this.showAlert('Advice', 'warning', error.error);
        else this.showAlert('Something bad happened', 'error', 'Unknown error!');
      },
    });
  }

  private showAlert(title: string, type: 'error' | 'success' | 'warning', message: string, istimer = false) {
    swal.fire({
      icon: type,
      title: title,
      text: message,
      timer: istimer ? 1500 : undefined,
      showConfirmButton: !istimer,
    });
  }
}
