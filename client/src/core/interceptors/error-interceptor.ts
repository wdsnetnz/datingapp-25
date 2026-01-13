import { HttpInterceptorFn } from '@angular/common/http';
import { ToastService } from '../services/toast-service';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toast = inject(ToastService);
  const router = inject(Router);
  
  return next(req).pipe(
    catchError((error => {
      if (error) {
        switch (error.status) {
          case 400:
            if(error.error.errors) {
              const modalStateErrors = [];
              for (const key in error.error.errors) {
                if (error.error.errors[key]) {
                  modalStateErrors.push(error.error.errors[key]);
                }
              }
              //toast.error(modalStateErrors.join(', '));
              throw modalStateErrors.flat();
            } else {
              toast.error(error.error );
            }
            break;

          case 404:
            router.navigateByUrl('/not-found');
            break;
          
          case 500:
            const navigationExtras: NavigationExtras = { state: { error: error.error } };
            router.navigateByUrl('/server-error', navigationExtras);
                     
            //toast.error('Server error');
            break;
          case 401:
            toast.error('Unauthorized');
            router.navigateByUrl('/login');
            break;
          default:
            toast.error('An error occurred');
            break;
        }
      }
      throw error;
    })
  ));
};