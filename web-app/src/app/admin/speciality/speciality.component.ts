import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DrawerComponent } from '../../shared/components/drawer/drawer.component';
import { FormComponent } from './components/form/form.component';
import { SpecialityListComponent } from './components/speciality-list/speciality-list.component';

@Component({
  selector: 'app-speciality',
  standalone: true,
  imports: [CommonModule, DrawerComponent, FormComponent, SpecialityListComponent],
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
