import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../core/services/auth.service';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
})
export class AuthComponent {
  form: FormGroup | undefined;
  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService) {
    this.buildForm();
  }

  private buildForm(): void {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

  submit(ev: Event):void {
    ev.preventDefault();
    if(!this.form?.valid){
      console.log('The form is invalid!');
      return;
    }
    const user = {...this.form.value};
    this.authService.login(user.email, user.password).subscribe(p => {
      console.log(p);
    });
    console.log(`Successful login!: ${user.email} ${user.password}`);
    alert('Successful login!');
  }
}
