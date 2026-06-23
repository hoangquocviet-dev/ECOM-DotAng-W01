import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { inject } from '@angular/core';
// Import ToastService hoặc NotificationService vào đây sau khi tạo

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMsg = 'Đã có lỗi xảy ra. Vui lòng thử lại sau.';
      
      if (error.error instanceof ErrorEvent) {
        // Lỗi từ phía Client
        errorMsg = `Lỗi Client: ${error.error.message}`;
      } else {
        // Lỗi từ phía Server Backend
        if (error.status === 401) {
          errorMsg = 'Phiên đăng nhập đã hết hạn.';
          // Có thể gọi AuthService.logout() ở đây
        } else if (error.status === 403) {
          errorMsg = 'Bạn không có quyền thực hiện thao tác này.';
        } else if (error.status >= 500) {
          errorMsg = 'Hệ thống Backend đang bận, vui lòng thử lại sau.';
        } else if (error.status === 400) {
          errorMsg = 'Dữ liệu không hợp lệ.';
        }
      }
      
      // Hiển thị Global Error Feedback (TODO: Replace with ToastService)
      console.error('Global Error Catcher: ', errorMsg);
      // alert(errorMsg); 
      
      return throwError(() => error);
    })
  );
};
