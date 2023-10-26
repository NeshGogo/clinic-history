import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SpecialityCreateDto } from 'src/app/core/models/speciality';
import { SpecialityService } from 'src/app/core/services/speciality.service';

@Component({
  selector: 'app-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './form.component.html',
})
export class FormComponent {
  form: FormGroup = this.formBuilder.group({
    name: ['', [Validators.required, Validators.maxLength(128)]],
    description: ['', [Validators.maxLength(800)]],
  });

  constructor(private formBuilder: FormBuilder, private service: SpecialityService) {}

  submit(ev: Event): void {
    ev.preventDefault();
    
    if (!this.form?.valid) {
      console.log('The form is invalid!');
      return;
    }
    
    const speciality: SpecialityCreateDto = { ...this.form.value };
    this.service.add(speciality).subscribe(speciality => console.log(speciality))
    // TODO: conectect to specialityService.
  }
}
