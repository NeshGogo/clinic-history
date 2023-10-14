import { Routes } from '@angular/router';
import { NotFoundComponent } from './components/not-found-componet/not-found.component';
import { AuthComponent } from './auth/auth.component';

export const routes: Routes = [
  {
    path: '',
    component: AuthComponent,
  },
  {
    path:'**',
    component: NotFoundComponent,
  }
];
