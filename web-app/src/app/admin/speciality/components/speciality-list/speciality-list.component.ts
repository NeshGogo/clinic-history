import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableComponent } from 'src/app/shared/components/table/table.component';
import { Speciality } from 'src/app/core/models/speciality';

@Component({
  selector: 'app-speciality-list',
  standalone: true,
  imports: [CommonModule, TableComponent],
  template: `
    <div>
      <app-table [headers]="headers" [items]="items">
        @for (item of items; track $index) {
        <tr class="bg-white border-b cursor-pointer" (click)="onClick(item)">
          <td class="px-6 py-4">{{ $index + 1 }}</td>
          <td class="px-6 py-4">
            @if (item.active) {
            <span class="material-symbols-outlined text-green-700">
              toggle_on
            </span>
            } @else {
            <span class="material-symbols-outlined text-red-700">
              toggle_off
            </span>
            }
          </td>
          <td class="px-6 py-4">{{ item.name }}</td>
          <td class="px-6 py-4">{{ item.description || '' }}</td>
        </tr>
        }@empty {
        <tr>
          <td class="px-6 py-4">There aren't any value....</td>
        </tr>
        }
      </app-table>
    </div>
  `,
})
export class SpecialityListComponent {
  headers = ['#', 'active', 'Name', 'Description'];
  @Input() items: Speciality[] = [];
  @Output() itemClick = new EventEmitter<Speciality>();

  onClick(speciality: Speciality) {
    this.itemClick.emit(speciality);
  }
}
