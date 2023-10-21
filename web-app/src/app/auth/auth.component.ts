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
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, AlertComponent],
  templateUrl: './auth.component.html',
})
export class AuthComponent {
  form: FormGroup | undefined;
  displayAlert: boolean = false;
  alertContent: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private route: Router
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
      next: () => {
        this.route.navigate(['/home']);
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
