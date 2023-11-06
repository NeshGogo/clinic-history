import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DrawerComponent } from 'src/app/shared/components/drawer/drawer.component';
import { FormComponent } from './components/form/form.component';
import { Doctor } from 'src/app/core/models/doctor';
import { DoctorService } from 'src/app/core/services/doctor.service';
import { DoctorListComponent } from './components/doctor-list/doctor-list.component';
import { PaginationComponent } from 'src/app/shared/components/pagination/pagination.component';
import swal from 'sweetalert2';
import { HttpErrorResponse } from '@angular/common/http';
import { DisableEnableZoneComponent } from 'src/app/shared/components/disable-enable-zone/disable-enable-zone.component';

@Component({
  selector: 'app-doctor',
  standalone: true,
  imports: [
    CommonModule,
    DrawerComponent,
    FormComponent,
    DoctorListComponent,
    PaginationComponent,
    DisableEnableZoneComponent,
  ],
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
        <app-disable-enable-zone
          *ngIf="doctor"
          (btnClick)="onDisabledOrEnable()"
          [isDisabled]="!doctor.active"
        ></app-disable-enable-zone>
      </div>
    </app-drawer>

    <div>
      <app-doctor-list
        [items]="doctors"
        (itemClick)="onItemClick($event)"
      ></app-doctor-list>
      <div class="mt-4">
        <app-pagination
          [currentPage]="currentPage"
          [itemsPerPage]="itemsPerPage"
          [totalItems]="totalItems"
          (pageChanged)="onPageChange($event)"
        ></app-pagination>
      </div>
    </div>
  `,
})
export class DoctorComponent implements OnInit {
  title = 'Doctors';
  formTitle = 'Add a new doctor';
  displayForm = false;
  doctor: Doctor | null = null;
  doctors: Doctor[] = [];
  currentPage: number = 1;
  numberOfPages: number = 0;
  itemsPerPage: number = 6;
  totalItems: number = 0;

  constructor(private service: DoctorService) {}

  ngOnInit(): void {
    this.fetchData();
  }

  fetchData() {
    this.service.getAll().subscribe((results) => {
      this.totalItems = results.length;
      this.numberOfPages = Math.ceil(this.totalItems / this.itemsPerPage);
      this.currentPage, this.itemsPerPage;
      this.doctors = results.slice(
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

  onPageChange(page: number): void {
    this.currentPage = page;
    this.fetchData();
  }

  onItemClick(doctor: Doctor) {
    this.doctor = doctor;
    this.openForm();
  }

  onDisabledOrEnable() {
    this.service.ActiveOrDisactive(this.doctor?.id as string).subscribe({
      next: () => {
        this.fetchData();
        this.showAlert(
          'Successed',
          'success',
          'Doctor updated successful!',
          true
        );
        this.displayForm = false;
        this.doctor = null;
      },
      error: (error: HttpErrorResponse) => {
        if (error.status == 400)
          this.showAlert('Advice', 'warning', error.error);
        else
          this.showAlert('Something bad happened', 'error', 'Unknown error!');
      },
    });
  }

  private showAlert(
    title: string,
    type: 'error' | 'success' | 'warning',
    message: string,
    istimer = false
  ) {
    swal.fire({
      icon: type,
      title: title,
      text: message,
      timer: istimer ? 1500 : undefined,
      showConfirmButton: !istimer,
    });
  }
}
