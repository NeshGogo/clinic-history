import { Component, OnInit } from '@angular/core';
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
  templateUrl: './speciality.component.html',
})
export class SpecialityComponent implements OnInit {
  title = 'Specialities';
  displayForm = false;
  formTitle = 'Add a new speciality';
  specialities: Speciality[] = [];
  speciality: Speciality | null = null;
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
      this.specialities = results.slice(
        this.currentPage - 1,
        this.currentPage + this.itemsPerPage
      );
    });
  }

  onSave() {
    this.fetchData();
    this.displayForm = false;
    this.speciality = null;
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.fetchData();
  }

  onItemClick(speciality: Speciality) {
    this.speciality = speciality;
    this.openForm();
  }

  onCloseDrawer() {
    this.speciality = null;
  }

  onDisabledOrEnable() {
    this.specialityService.ActiveOrDisactive(this.speciality?.id as string)
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
        this.speciality = null;
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
