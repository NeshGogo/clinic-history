import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableComponent } from 'src/app/shared/components/table/table.component';
import { Doctor } from 'src/app/core/models/doctor';

@Component({
  selector: 'app-doctor-list',
  standalone: true,
  imports: [CommonModule, TableComponent],
  template: `
    <app-table [headers]="headers" [items]="items">
      <ng-container *ngIf="items.length > 0; else elseBlock">
        <tr
          *ngFor="let item of items"
          class="bg-white border-b cursor-pointer"
          (click)="onClick(item)"
        >
          <td class="px-6 py-4">
            <span
              *ngIf="item.active"
              class="material-symbols-outlined text-green-700"
            >
              toggle_on
            </span>
            <span
              *ngIf="!item.active"
              class="material-symbols-outlined text-red-700"
            >
              toggle_off
            </span>
          </td>
          <td class="px-6 py-4">{{ item.fullName }}</td>
          <td class="px-6 py-4">{{ item.identification }}</td>
          <td class="px-6 py-4">{{ item.speciality?.name || ''}}</td>
        </tr>
      </ng-container>
      <ng-template #elseBlock>
        <tr>
          <td class="px-6 py-4">There aren't any value....</td>
        </tr>
      </ng-template>
    </app-table>
  `,
})
export class DoctorListComponent {
  headers = ['Active', 'Full Name', 'Identification', 'Speciality'];
  @Input() items: Doctor[] = [];
  @Output() itemClick = new EventEmitter<Doctor>()

  onClick(doctor: Doctor){
    this.itemClick.emit(doctor);
  }
}
