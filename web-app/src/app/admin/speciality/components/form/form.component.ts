import { Component, EventEmitter, Output } from '@angular/core';
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
import swal  from 'sweetalert2';

@Component({
  selector: 'app-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './form.component.html',
})
export class FormComponent {
  @Output() OnSave: EventEmitter<Speciality> = new EventEmitter();
  form: FormGroup = this.formBuilder.group({
    name: ['', [Validators.required, Validators.maxLength(128)]],
    description: ['', [Validators.maxLength(800)]],
  });

  constructor(
    private formBuilder: FormBuilder,
    private service: SpecialityService
  ) {}

  submit(ev: Event): void {
    ev.preventDefault();

    if (!this.form?.valid) {
      console.log('The form is invalid!');
      return;
    }
    const speciality: SpecialityCreateDto = { ...this.form.value };
    this.service.add(speciality).subscribe({
      next: (speciality) => {
        this.form.reset();
        this.OnSave.emit(speciality);
        this.showAlert('Successed', 'success', 'Speciality added successful!');
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
    message: string
  ) {
    swal.fire({
      icon: type,
      title: title,
      text: message,
    });
  }
}
