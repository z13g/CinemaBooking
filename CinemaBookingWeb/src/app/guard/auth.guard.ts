
import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (authService.isAuthenticated()) {
        console.log("user is authenticated")
        return true;
    } else {
        console.log("user is not authenticated")
        // router.navigate(['/login']);
        return false;
    }
};

export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticated() && authService.getUserRole() === 'Admin') {
      console.log("user is admin")
      return true;
  } else {
      console.log("user is not admin")
      router.navigate(['/unauthorized']);
      return false;
  }
};


// import { CanActivateFn } from '@angular/router';
// import { Router } from '@angular/router';
// import { inject } from '@angular/core';
// import { catchError, map } from 'rxjs/operators';
// import { of } from 'rxjs';
// import { UserService } from '../services/generic.services'

// export const authGuard: CanActivateFn = (route, state) => {
//   const service = inject(UserService);
//   const router = inject(Router);

//   const authToken = service.getAuthToken();

//   if (!authToken) {
//     router.navigate(['/login']);
//     return of(false);
//   }

//   return service.validateToken(authToken).pipe(
//     map(userDetails => {
//       if (userDetails) {
//         return true;
//       }
//       router.navigate(['/login']);
//       return false;
//     }),
//     catchError(() => {
//       router.navigate(['/login']);
//       return of(false);
//     })
//   );
// };

// export const adminGuard: CanActivateFn = (route, state) => {
//   const service = inject(UserService);
//   const router = inject(Router);

//   const authToken = service.getAuthToken();

//   if (!authToken) {
//     router.navigate(['/login']);
//     return of(false);
//   }

//   return service.validateToken(authToken).pipe(
//     map(userDetails => {
//       if (userDetails && userDetails.role === 'admin') {
//         return true;
//       }
//       router.navigate(['/unauthorized']);
//       return false;
//     }),
//     catchError(() => {
//       router.navigate(['/login']);
//       return of(false);
//     })
//   );
// };
