import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableComponent } from 'src/app/shared/components/table/table.component';
import { Speciality } from 'src/app/core/models/speciality';

@Component({
  selector: 'app-speciality-list',
  standalone: true,
  imports: [CommonModule, TableComponent],
  templateUrl: './speciality-list.component.html',
})
export class SpecialityListComponent {
  headers = ['Name', 'Description'];
  @Input() items: Speciality[] = [];
  @Output() itemClick = new EventEmitter<Speciality>()

  onClick(speciality: Speciality){
    this.itemClick.emit(speciality);
  }
}
