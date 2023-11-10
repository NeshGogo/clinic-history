import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DrawerComponent } from '../../shared/components/drawer/drawer.component';
import { FormComponent } from './components/form/form.component';
import { SpecialityListComponent } from './components/speciality-list/speciality-list.component';
import { SpecialityService } from 'src/app/core/services/speciality.service';
import { Speciality } from 'src/app/core/models/speciality';
import { PaginationComponent } from 'src/app/shared/components/pagination/pagination.component';
import { Title } from '@angular/platform-browser';
import { DisableEnableZoneComponent } from 'src/app/shared/components/disable-enable-zone/disable-enable-zone.component';
import { HttpErrorResponse } from '@angular/common/http';
import swal from 'sweetalert2';

@Component({
  selector: 'app-speciality',
  standalone: true,
  imports: [
    CommonModule,
    DrawerComponent,
    FormComponent,
    SpecialityListComponent,
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
        class="leading-none text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-4 focus:ring-green-300 font-medium rounded-full text-lg px-1.5 py-1 text-center mr-2 mb-2"
      >
        <span class="material-symbols-outlined"> add </span>
      </button>
    </div>
    <app-drawer
      [title]="formTitle"
      [(open)]="displayForm"
      (onClose)="onCloseDrawer()"
    >
      @if (displayForm) {
      <div class="flex justify-between flex-col drawer-container">
        <app-form (OnSave)="onSave()" [speciality]="speciality()"></app-form>
        @if (speciality()) {
        <app-disable-enable-zone
          (btnClick)="onDisabledOrEnable()"
          [isDisabled]="!speciality()?.active"
        ></app-disable-enable-zone>
        }
      </div>
      }
    </app-drawer>
    <div>
      <app-speciality-list
        [items]="specialities()"
        (itemClick)="onItemClick($event)"
      ></app-speciality-list>
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
export class SpecialityComponent implements OnInit {
  title = 'Specialities';
  displayForm = false;
  formTitle = 'Add a new speciality';
  specialities = signal<Speciality[]>([]);
  speciality = signal<Speciality | null>(null);
  currentPage: number = 1;
  numberOfPages: number = 0;
  itemsPerPage: number = 6;
  totalItems: number = 0;

  constructor(
    private specialityService: SpecialityService,
    titleService: Title
  ) {
    titleService.setTitle('NC | Admin-Specialities');
  }

  ngOnInit(): void {
    this.fetchData();
  }

  openForm() {
    this.displayForm = true;
  }

  fetchData() {
    this.specialityService.getAll().subscribe((results) => {
      this.totalItems = results.length;
      this.numberOfPages = Math.ceil(this.totalItems / this.itemsPerPage);
      this.currentPage, this.itemsPerPage;
      this.specialities.set(
        results.slice(
          this.currentPage == 1
            ? 0
            : this.itemsPerPage * (this.currentPage - 1),
          this.currentPage * this.itemsPerPage
        )
      );
    });
  }

  onSave() {
    this.fetchData();
    this.displayForm = false;
    this.speciality.set(null);
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.fetchData();
  }

  onItemClick(speciality: Speciality) {
    this.speciality.set(speciality);
    this.openForm();
  }

  onCloseDrawer() {
    this.speciality.set(null);
  }

  onDisabledOrEnable() {
    this.specialityService
      .ActiveOrDisactive(this.speciality()?.id as string)
      .subscribe({
        next: () => {
          this.fetchData();
          this.showAlert(
            'Successed',
            'success',
            'Speciality updated successful!',
            true
          );
          this.displayForm = false;
          this.speciality.set(null);
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
