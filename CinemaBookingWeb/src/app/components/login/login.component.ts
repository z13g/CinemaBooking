import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm!: FormGroup;
  signUpForm!: FormGroup;
  error: string = '';
  constructor(private formBuilder: FormBuilder, private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    console.log("login site init")
    this.loginForm = this.formBuilder.group({
      email: [''],
      password: [''], 
    });
    this.signUpForm = this.formBuilder.group({
      email: [''], 
      password: [''],
      retypePassword: [''],
      name: [''],
      phoneNumber: [''],
    });
    console.log("login after form create")
  }

  onLogin() {
    if (this.loginForm.valid) {
        const { email, password } = this.loginForm.value;
        console.log("email:", email, "password:", password);
        this.authService.login(email, password).subscribe({
            next: (success) => {
                if (success) {
                    console.log("Login success from login component");
                    this.router.navigate(['/']);
                } else {
                    this.error = 'Invalid credentials';
                    console.log("Invalid credentials from login component");
                }
            },
            error: (err) => {
                this.error = err.message;
                console.log("new error test", err)
                const errorField = document.getElementById("login-cred-error");
                if (errorField) {
                  const child = errorField.firstChild;
                  if (child instanceof HTMLElement) { 
                      child.classList.add('show');
                      child.textContent = err;
                  }
                }
            }
        });
    }
  }

  onSignUp() {
    if (this.signUpForm.valid) {
        const { email, password, retypePassword, phoneNumber, name } = this.signUpForm.value;
        let error = false;        
        if (!this.checkPasswordStrength(password)) {
          this.displayError('password', 'Adgangskoden er ikke stærk nok');
          error = true;
        }
        if (!this.checkPasswordMatch(password, retypePassword)) {
          console.log("Passwords do not match")
          error = true;
        }
        console.log("phone:", phoneNumber, "email:", email, "password:", password, "name:", name)
        if (!this.checkDanishPhoneNumber(phoneNumber)) {
          console.log("Invalid Danish phone number format")
          error = true;
        }
        if (!this.checkEmail(email)) {
          console.log("Invalid email format")
          error = true;
        }
        if (!this.checkName(name)) {
          console.log("Name must contain only alphabets")
          error = true;
        }

        if (!error) {
            console.log("Signup data is valid");
            this.authService.signup(name, email, phoneNumber, password).subscribe({
                next: (success) => {
                    if (success) {
                        console.log("Signup success from login component");
                        this.router.navigate(['/']);
                    } else {
                        this.error = 'Signup failed';
                        console.log("Signup failed from login component");
                    }
                },
                error: (err) => {
                  this.error = err.message;
                  console.log("new signup error test", err)
                  const errorField = document.getElementById("signup-cred-error");
                  if (errorField) {
                    const child = errorField.firstChild;
                    if (child instanceof HTMLElement) { 
                        child.classList.add('show');
                        child.textContent = err;
                    }
                  }
                }
            });
        }
    }
  }


  showSignupDesign(): void {
    // const showSignUpSection = document.getElementById('show-signup-section');
    const signUpSection = document.getElementById('signup-section');
    const signinSection = document.getElementById('signin-section');
    // if (showSignUpSection && signUpSection && signinSection) {
    if (signUpSection && signinSection) {
      if (signUpSection.classList.contains('show')) {
        signUpSection.classList.remove('show');
        // showSignUpSection.classList.add('show');
        signinSection.classList.add('show');
        return;
      }
      else {
        signUpSection.classList.add('show');
        // showSignUpSection.classList.remove('show');
        signinSection.classList.remove('show');
        return;
      }
    }
  }

  onInputChange(inputField: EventTarget | null) {
    if (inputField instanceof HTMLInputElement) {
      if (inputField.name === 'password') {
        console.log('Password input field:', inputField.value);
        const valid = this.checkPasswordStrength(inputField.value);
        if (valid) {
          inputField.style.borderColor = 'green';
        } else {
          inputField.style.borderColor = 'red';
        }
      }
      else if (inputField.name === 'name') {
        console.log('Name input field:', inputField.value);
        const valid = this.checkName(inputField.value);
        if (valid) {
          inputField.style.borderColor = 'green';
        }
        else {
          inputField.style.borderColor = 'red';
        }
      }
      else if (inputField.name === 'email') {
        const valid = this.checkEmail(inputField.value);
        if (valid) {
          inputField.style.borderColor = 'green';
        }
        else {
          inputField.style.borderColor = 'red';
        }
      }
      else if (inputField.name === 'phone') {
        const valid = this.checkDanishPhoneNumber(inputField.value);
        if (valid) {
          inputField.style.borderColor = 'green';
        }
        else {
          inputField.style.borderColor = 'red';
        }
      }
      else if (inputField.name === 'retypePassword') {
        const valid = this.checkPasswordMatch(this.signUpForm.value.password, inputField.value);
        if (valid) {
          inputField.style.borderColor = 'green';
        }
        else {
          inputField.style.borderColor = 'red';
        }
      }
    } else {
      console.error('Ugyldig inputfelt.');
    }
  }

  checkDanishPhoneNumber(phoneNumber: string): boolean {
    // if (phoneNumber && phoneNumber.length > 0) {
    //     const regex = /^[2-9]\d{7}$/;
    //     if (regex.test(phoneNumber)) {
    //         this.displayError('phone', null);
    //         return true;
    //     }
    //     console.log("Invalid Danish phone number format");
    //     this.displayError('phone', 'Ugyldigt dansk telefonnummerformat');
    //     return false;
    // } else {
    //     console.log("Phone number is required");
    //     this.displayError('phone', 'Telefonnummer er påkrævet');
    //     return false;
    // }
    return true;
}

  checkName(name: string): boolean {
    if (name && name.length > 0) {
      const regex = /^[A-Za-z]+$/;
      if (regex.test(name)) {
        this.displayError('name', null);
        return true;
      }
      console.log("Name must contain only alphabets")
      this.displayError('name', 'Navn må kun indeholde alfabetiske tegn');
      return false;
    }
    else {
      console.log("Name is required")
      this.displayError('name', 'Navn er påkrævet');
      return false;
    }
  }

  checkEmail(email: string): boolean {
    if (email && email.length > 0) {
        // Regular expression for validating an email
        const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (regex.test(email)) {
            this.displayError('email', null);
            return true;
        }
        console.log("Invalid email format");
        this.displayError('email', 'Ugyldigt e-mail-format');
        return false;
    } else {
        console.log("Email is required");
        this.displayError('email', 'E-mail er påkrævet');
        return false;
    }
}

checkPasswordMatch(password: string, retypePassword: string): boolean {
    if (password === retypePassword) {
        this.displayError('retypePassword', null);
        return true;
    }
    console.log("Passwords do not match");
    this.displayError('retypePassword', 'Adgangskoderne matcher ikke');
    return false;
}


  displayError(errorFieldName: string | null, message: string | null) {
    const errorField = document.getElementById(`${errorFieldName}-error`);
    if (!message) {
      if (errorField) {
        errorField.classList.remove('show');
      }
      return;
    }
    if (errorField) {
      errorField.innerText = message;
      errorField.classList.add('show');
    }
  }

  onInputFocus(inputField: EventTarget | null) {
    if (inputField instanceof HTMLInputElement) {
      console.log('Input felt ID:', inputField.name);
      if (inputField.name === 'password') {
        console.log('Password input field focused');
        const passwordReq = document.getElementById('password-requirements');
        if (passwordReq) {
          passwordReq.classList.add('show');
        }
      }
    } else {
      console.error('Ugyldig inputfelt.');
    }
  }

  onInputBlur(inputField: EventTarget | null) {
    if (inputField instanceof HTMLInputElement) {
      if (inputField.name === 'password') {
        console.log('Password input field blurred');
        const passwordReq = document.getElementById('password-requirements');
        if (passwordReq) {
          passwordReq.classList.remove('show');
        }
      }
    } else {
      console.error('Ugyldig inputfelt.');
    }
  }

  checkPasswordStrength(password: string): boolean {
    const minLength = 8;
    const hasUppercase = /[A-Z]/.test(password || '');
    const hasLowercase = /[a-z]/.test(password || '');
    const hasNumber = /\d/.test(password || '');
    const hasSpecialChar = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/.test(password || '');
  
    const passLengthSpan = document.getElementById('pass-length-span');
    const passDigitSpan = document.getElementById('pass-digit-span');
    const passUppercaseSpan = document.getElementById('pass-uppercase-span');
    const passLowercaseSpan = document.getElementById('pass-lowercase-span');
    const passSymbolSpan = document.getElementById('pass-symbol-span');
    let cnt = 0;
  
    if (password && password.length >= minLength) {
      if (passLengthSpan) {
        passLengthSpan.innerText = 'Yes';
        cnt++;
      }
    }
    else {
      if (passLengthSpan) {
        passLengthSpan.innerText = 'No';
      }
    }
  
    if (hasUppercase) {
      if (passUppercaseSpan) {
        passUppercaseSpan.innerText = 'Yes';
        cnt++;
      }
    }
    else {
      if (passUppercaseSpan) {
        passUppercaseSpan.innerText = 'No';
      }
    }
  
    if (hasLowercase) {
      if (passLowercaseSpan) {
        passLowercaseSpan.innerText = 'Yes';
        cnt++;
      }
    }
    else {
      if (passLowercaseSpan) {
        passLowercaseSpan.innerText = 'No';
      }
    }
  
    if (hasNumber) {
      if(passDigitSpan) {
        passDigitSpan.innerText = 'Yes';
        cnt++;
      }
    }
    else {
      if (passDigitSpan) {
        passDigitSpan.innerText = 'No';
      }
    }
  
    if (hasSpecialChar) {
      if (passSymbolSpan) {
        passSymbolSpan.innerText = 'Yes';
        cnt++;
      }
    }
    else {
      if (passSymbolSpan) {
        passSymbolSpan.innerText = 'No';
      }
    }
  
    console.log('cnt:', cnt);
    return cnt === 5;
  }

}
