import { Component, EventEmitter, OnInit, Output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DoctorService } from 'src/app/core/services/doctor.service';
import { Doctor } from 'src/app/core/models/doctor';
import { PatientService } from 'src/app/core/services/patient.service';
import { mergeMap, of, switchMap } from 'rxjs';
import { PatientCreateDto } from 'src/app/core/models/patient';
import { ClinicRecord, ClinicRecordCreateDto } from 'src/app/core/models/clinicRecord';
import { ClinicRecordService } from 'src/app/core/services/clinic-record.service';
import swal from 'sweetalert2';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-clinic-record-with-patient-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <form [formGroup]="form" (ngSubmit)="submit($event)">
      <div class="mb-2">
        <label for="" class="block text-sm font-medium leading-6 text-gray-900">Full Name</label>
        <input
          name="fullName"
          type="text"
          placeholder="Type a specialty"
          formControlName="fullName"
          class="block w-full rounded-md border-0 py-1.5 pl-7 pr-20 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
        />
        @if (form.get('fullName')?.touched && form.get('fullName')?.hasError('required')) {
        <p class="text-red-700">* This field is requiered.</p>
        } @if (form.get('fullName')?.touched && form.get('fullName')?.hasError('maxLength')) {
        <p class="text-red-700">* The max length is 128 characters.</p>
        }
      </div>
      <div class="mb-2">
        <label for="" class="block text-sm font-medium leading-6 text-gray-900">Identification</label>
        <input
          name="identification"
          type="text"
          placeholder="Type the identification"
          formControlName="identification"
          class="block w-full rounded-md border-0 py-1.5 pl-7 pr-20 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
        />
        @if(form.get('identification')?.touched && form.get('identification')?.hasError('required')){
        <p class="text-red-700">* This field is requiered.</p>
        } @if (form.get('identification')?.touched && (form.get('identification')?.hasError('maxLength') ||
        form.get('identification')?.hasError('minLength'))) {
        <p class="text-red-700">* The length should be 11 characters.</p>
        }
      </div>
      <div class="mb-2">
        <label for="sex" class="block mb-2 text-sm font-medium text-gray-900">Sex</label>
        <select
          id="sex"
          formControlName="sex"
          class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
        >
          <option [value]="null">Choose a sex</option>
          <option value="male">Male</option>
          <option value="male">Female</option>
        </select>
        @if (form.get('sex')?.touched && form.get('sex')?.hasError('nullValidator')) {
        <p class="text-red-700">* This field is requiered.</p>
        }
      </div>
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
export class ClinicRecordWithPatientFormComponent implements OnInit {
  @Output() OnSave: EventEmitter<ClinicRecord> = new EventEmitter();
  doctors = signal<Doctor[]>([]);
  form: FormGroup = this.formBuilder.group({
    fullName: ['', [Validators.required, Validators.maxLength(128)]],
    identification: ['', [Validators.required, Validators.maxLength(11), Validators.minLength(11)]],
    sex: [null, [Validators.required]],
    doctorId: [null, [Validators.required, Validators.maxLength(36), Validators.minLength(36), Validators.nullValidator]],
    diagnosis: ['', [Validators.maxLength(800)]],
  });

  constructor(
    private formBuilder: FormBuilder,
    private doctorService: DoctorService,
    private patientService: PatientService,
    private recordService: ClinicRecordService
  ) {}

  ngOnInit(): void {
    this.fetchDoctors();
  }

  fetchDoctors() {
    this.doctorService.getInHistory().subscribe((p) => this.doctors.set(p));
  }

  submit(ev: Event): void {
    ev.preventDefault();

    if (!this.form?.valid) {
      console.log('The form is invalid!');
      return;
    }
    const newPatient: PatientCreateDto = {
      fullName: this.form.value.fullName,
      identification: this.form.value.identification,
      sex: this.form.value.sex,
    };
    this.patientService
      .exists(newPatient.identification)
      .pipe(
        switchMap((exists) => (exists ? of(null) : this.patientService.add(newPatient))),
        mergeMap((patient) => {
          if (patient === null) return of(null);
          const body: ClinicRecordCreateDto = {
            patientId: patient.id,
            doctorId: this.form.value.doctorId,
            diagnosis: this.form.value.diagnosis,
          };
          return this.recordService.add(body);
        })
      )
      .subscribe({
        next: (record) => {
          if (record == null) {
            this.showAlert('Advice', 'warning', 'Patient already exists');
          } else {
            this.showAlert('Successed', 'success', 'Patient added successful!', true);
            this.OnSave.emit(record);
          }
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
