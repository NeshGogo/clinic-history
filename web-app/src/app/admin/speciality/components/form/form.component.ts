import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  Speciality,
  SpecialityCreateDto,
} from 'src/app/core/models/speciality';
import { SpecialityService } from 'src/app/core/services/speciality.service';
import { HttpErrorResponse } from '@angular/common/http';
import swal from 'sweetalert2';

@Component({
  selector: 'app-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './form.component.html',
})
export class FormComponent implements OnInit {
  @Input() speciality: Speciality | null = null;
  @Output() OnSave: EventEmitter<Speciality> = new EventEmitter();
  form: FormGroup = this.formBuilder.group({
    name: ['', [Validators.required, Validators.maxLength(128)]],
    description: ['', [Validators.maxLength(800)]],
  });

  constructor(
    private formBuilder: FormBuilder,
    private service: SpecialityService
  ) {}

  ngOnInit(): void {
    if (this.speciality) {
      this.setForm();
    }
  }

  private setForm() {
    this.form.reset();
    this.form.patchValue(this.speciality as Speciality);
  }

  submit(ev: Event): void {
    ev.preventDefault();

    if (!this.form?.valid) {
      console.log('The form is invalid!');
      return;
    }

    const speciality: SpecialityCreateDto = { ...this.form.value };
    if (this.speciality) {
      this.update(speciality);
    } else {
      this.add(speciality);
    }
  }

  private add(createDto: SpecialityCreateDto) {
    this.service.add(createDto).subscribe({
      next: (speciality) => {
        this.form.reset();
        this.OnSave.emit(speciality);
        this.showAlert(
          'Successed',
          'success',
          'Speciality added successful!',
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

  private update(createDto: SpecialityCreateDto) {
    const id: string = this.speciality?.id as string;
    this.service.update(id, createDto).subscribe({
      next: (speciality) => {
        this.form.reset();
        this.OnSave.emit(speciality);
        this.showAlert(
          'Successed',
          'success',
          'Speciality updated successful!',
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
