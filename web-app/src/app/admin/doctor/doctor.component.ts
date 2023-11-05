import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DrawerComponent } from 'src/app/shared/components/drawer/drawer.component';
import { FormComponent } from './components/form/form.component';
import { Doctor } from 'src/app/core/models/doctor';
import { DoctorService } from 'src/app/core/services/doctor.service';

@Component({
  selector: 'app-doctor',
  standalone: true,
  imports: [CommonModule, DrawerComponent, FormComponent],
  template: `
    <h1
      class="mb-4 text-lg font-extrabold leading-none tracking-tight text-gray-700 md:text-xl lg:text-2xl text-center"
    >
      {{ title }}
    </h1>

    <div class="text-right">
      <button
        type="button"
        (click)="openForm()"
        class="leading-none  text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-4 focus:ring-green-300 font-medium rounded-full text-lg px-1.5 py-1 text-center mr-2 mb-2"
      >
        <span class="material-symbols-outlined"> add </span>
      </button>
    </div>

    <app-drawer
  [title]="formTitle"
  [(open)]="displayForm"
  (onClose)="onCloseDrawer()"
>
  <div class="flex justify-between flex-col drawer-container">
    <app-form
      *ngIf="displayForm"
      (OnSave)="onSave()"
      [doctor]="doctor"
    ></app-form>
    <!-- <app-disable-enable-zone
      *ngIf="speciality"
      (btnClick)="onDisabledOrEnable()"
      [isDisabled]="!speciality.active"
    ></app-disable-enable-zone> -->
  </div>
</app-drawer>

  `,
})
export class DoctorComponent {
  title = 'Doctors';
  formTitle = 'Add a new doctor';
  displayForm = false;
  doctor: Doctor | null = null;
  specialities: Doctor[] = [];
  currentPage: number = 1;
  numberOfPages: number = 0;
  itemsPerPage: number = 6;
  totalItems: number = 0;

constructor(private service: DoctorService){}

  fetchData() {
    this.service.getAll().subscribe((results) => {
      this.totalItems = results.length;
      this.numberOfPages = Math.ceil(this.totalItems / this.itemsPerPage);
      this.currentPage, this.itemsPerPage;
      this.specialities = results.slice(
        this.currentPage - 1,
        this.currentPage + this.itemsPerPage
      );
    });
  }

  onSave() {
    this.fetchData();
    this.displayForm = false;
    this.doctor = null;
  }

  openForm() {
    this.displayForm = true;
  }

  onCloseDrawer() {
    this.doctor = null;
  }
}
