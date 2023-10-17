import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { AuthService } from '../core/services/auth.service';
import { AlertComponent } from '../shared/components/alert/alert.component';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, AlertComponent],
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
})
export class AuthComponent {
  form: FormGroup | undefined;
  displayAlert: boolean = false;
  alertContent: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService
  ) {
    this.buildForm();
  }

  private buildForm(): void {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

  submit(ev: Event): void {
    ev.preventDefault();
    if (!this.form?.valid) {
      this.showAlert('The form is invalid!');
      return;
    }
    const user = { ...this.form.value };
    this.authService.login(user.email, user.password).subscribe({
      next: (userToke) => {
        console.log('token', userToke);
      },
      error: (error) => {
        if (error.status === 0) {
          this.showAlert('Verify your internet connection');
        } else {
          this.showAlert('Email or password icorrect...');
        }
      },
    });
  }

  showAlert(content: string) {
    this.displayAlert = true;
    this.alertContent = content;
    setTimeout(() => {
      this.displayAlert = false;
    }, 5000);
  }
}
