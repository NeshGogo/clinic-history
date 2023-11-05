import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Doctor, DoctorCreateDto } from 'src/app/core/models/doctor';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SpecialityService } from 'src/app/core/services/speciality.service';
import { Speciality } from 'src/app/core/models/speciality';
import swal from 'sweetalert2';
import { DoctorService } from 'src/app/core/services/doctor.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './form.component.html',
})
export class FormComponent implements OnInit{
  @Input() doctor: Doctor | null = null;
  @Output() OnSave: EventEmitter<Doctor> = new EventEmitter();
  specialities: Speciality[] = [];
  form: FormGroup = this.formBuilder.group({
    fullName: ['', [Validators.required, Validators.maxLength(128)]],
    identification: ['', [Validators.required, Validators.maxLength(11), Validators.minLength(11)]],
    specialityId: [null, [Validators.required, Validators.maxLength(36), Validators.minLength(36), Validators.nullValidator]],
  });

  constructor(
    private formBuilder: FormBuilder,
    private specialityService: SpecialityService,
    private service: DoctorService,
  ) {}

  ngOnInit(): void {
    this.fetchSpecialities();
    if (this.doctor) {
      this.setForm();
    }
  }

  fetchSpecialities() {
    this.specialityService
      .getPublic()
      .subscribe((p) => (this.specialities = p));
  }



  private setForm() {
    this.form.reset();
    this.form.patchValue(this.doctor as Doctor);
  }

  submit(ev: Event): void {
    ev.preventDefault();

    if (!this.form?.valid) {
      console.log('The form is invalid!');
      return;
    }

    const doctor: DoctorCreateDto = { ...this.form.value };
    if (this.doctor) {
      this.update(doctor);
    } else {
      this.add(doctor);
    }
  }

  private add(createDto: DoctorCreateDto) {
    this.service.add(createDto).subscribe({
      next: (doctor) => {
        this.form.reset();
        this.OnSave.emit(doctor);
        this.showAlert(
          'Successed',
          'success',
          'Doctor added successful!',
          true
        );
      },
      error: (error: HttpErrorResponse) => {
        if (error.status == 400)
          this.showAlert('Advice', 'warning', error.error);
        else
          this.showAlert('Something bad happened', 'error', 'Unknown error!');
      },
    });
  }

  private update(createDto: DoctorCreateDto) {
    const id: string = this.doctor?.id as string;
    this.service.update(id, createDto).subscribe({
      next: (doctor) => {
        this.form.reset();
        this.OnSave.emit(doctor);
        this.showAlert(
          'Successed',
          'success',
          'Doctor updated successful!',
          true
        );
      },
      error: (error: HttpErrorResponse) => {
        if (error.status == 400)
          this.showAlert('Advice', 'warning', error.error);
        else
          this.showAlert('Something bad happened', 'error', 'Unknown error!');
      },
    });
  }

  private showAlert(
    title: string,
    type: 'error' | 'success' | 'warning',
    message: string,
    istimer = false
  ) {
    swal.fire({
      icon: type,
      title: title,
      text: message,
      timer: istimer ? 1500 : undefined,
      showConfirmButton: !istimer,
    });
  }

}
