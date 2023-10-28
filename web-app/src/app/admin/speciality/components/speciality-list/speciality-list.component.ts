import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableComponent } from 'src/app/shared/components/table/table.component';

@Component({
  selector: 'app-speciality-list',
  standalone: true,
  imports: [CommonModule, TableComponent],
  templateUrl: './speciality-list.component.html',
})
export class SpecialityListComponent {
  headers = ['Name', 'Description'];
  items: unknown[] = ['test'];
}
