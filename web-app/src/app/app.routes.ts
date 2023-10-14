import { Routes } from '@angular/router';
import { NotFoundComponent } from './components/not-found-componet/not-found.component';

export const routes: Routes = [
  {
    path:'**',
    component: NotFoundComponent,
  }
];
