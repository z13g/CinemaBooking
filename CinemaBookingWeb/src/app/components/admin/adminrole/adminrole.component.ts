import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Role } from 'src/app/models/role/role';
import { GenericService } from 'src/app/services/generic.services';

@Component({
  selector: 'app-adminrole',
  templateUrl: './adminrole.component.html',
  styleUrls: ['./adminrole.component.css']
})
export class AdminroleComponent {
  roleForm!: FormGroup;
  roleList: Role[] =[];
  currentRole: Role | null = null;
  isEditMode: boolean = false;
  showForm: boolean = false;
  showList: boolean = true;

  initForm(role?: Role): void {
    this.roleForm = new FormGroup({
      roleID: new FormControl(role ? role.roleID : ''),
      roleName: new FormControl(role ? role.roleName : '', Validators.required),
    });
  }
  
  constructor(private roleService: GenericService<Role>) {
    this.initForm();
  }
    ngOnInit() {
      this.fetchRole();
      this.initForm();
    }
  
    fetchRole() {
      this.roleService.getAll("role").subscribe(data => {
        this.roleList = data;
        console.log("Role: ", this.roleList);
        
      });
    }
  
    editRole(role: Role): void {
      this.currentRole = role;
      console.log("CurrentRole: ", this.currentRole);
      this.isEditMode = true;
      this.showForm = true;
      this.showList = false;
      this.initForm(role);
    }
  
    resetForm(): void {
      this.roleForm.reset();
      this.showForm = false;
      this.showList = true;
      this.isEditMode = false;
      this.currentRole = null;
    }
  
    toggleSave(): void {
      this.isEditMode = false;
      this.currentRole = null;
      this.initForm();
      this.showForm = !this.showForm;
      this.showList = !this.showForm;
    }
  
    public save(): void {
      if (this.roleForm.valid) {
        const formdata = this.roleForm.value;
        
        const roleid = this.isEditMode ? formdata.roleID : 0;
  
        const roleData = {
          roleID: roleid,
          roleName: formdata.roleName,
        };

        if (this.isEditMode == true && roleData.roleID) {  
          console.log("TestDatA: ", roleData);
          
          this.roleService.update('Role', roleData, roleData.roleID).subscribe({
            next: (response) => {
              console.log('role updated:', response);
              this.resetForm();
              this.fetchRole();
            },
            error: (error) => {
              console.error('Failed to update role:', error);
              alert(`Failed to update role: ${error.error.title}`);
            }
          });
        }
           else
           {
            console.log("role Create: ", roleData);
            
              this.roleService.create('Role', roleData).subscribe({
                next: (response) => {
                  console.log('role saved:', response);
                  this.roleList.push(response);
                  this.roleForm.reset();
                  this.showForm = false;
                  this.showList = true;
                },
                error: (error) => {
                  console.error('Failed to create role:', error);
                  alert(`Failed to create role: ${error.error.title}`);
                }
              });
      }
    }
   }
    
    deleteRole(role: Role) {
      if (role && role.roleID !== undefined) {
          this.roleService.delete('role', role.roleID).subscribe(() => {
              this.fetchRole();
          });
      }
    }
  }

