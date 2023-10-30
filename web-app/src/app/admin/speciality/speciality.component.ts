import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DrawerComponent } from '../../shared/components/drawer/drawer.component';
import { FormComponent } from './components/form/form.component';
import { SpecialityListComponent } from './components/speciality-list/speciality-list.component';
import { SpecialityService } from 'src/app/core/services/speciality.service';
import { Speciality } from 'src/app/core/models/speciality';

@Component({
  selector: 'app-speciality',
  standalone: true,
  imports: [CommonModule, DrawerComponent, FormComponent, SpecialityListComponent],
  templateUrl: './speciality.component.html',
})
export class SpecialityComponent  implements OnInit {
  title = 'Specialities';
  displayForm = false;
  formTitle = 'Add a new speciality'
  specialities: Speciality[] = []

  constructor(private specialityService: SpecialityService ){}

  ngOnInit(): void {
    this.fetchData();
  }

  openForm() {
    this.displayForm = true;
  }

  fetchData() {
    this.specialityService.getAll()
    .subscribe(p => this.specialities = p);
  }

  onSave(){
    this.fetchData();
    this.displayForm =  false;
  }
}
