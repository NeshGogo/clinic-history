import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DrawerComponent } from '../shared/components/drawer/drawer.component';
import { FormComponent } from './ui/form/form.component';

@Component({
  selector: 'app-speciality',
  standalone: true,
  imports: [CommonModule, DrawerComponent, FormComponent],
  templateUrl: './speciality.component.html',
})
export class SpecialityComponent {
  title = 'Specialities';
  displayForm = false;
  formTitle = 'Add a new speciality'
  openForm() {
    this.displayForm = true;
  }
}
