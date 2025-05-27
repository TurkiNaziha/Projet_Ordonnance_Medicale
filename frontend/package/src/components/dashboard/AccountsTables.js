import { Card, CardBody, CardTitle, CardSubtitle, Table , Modal, Button ,  ModalHeader, ModalBody,ModalFooter,  Form,
  FormGroup,  Label,Input} from "reactstrap";
import user1 from "../../assets/images/users/user1.jpg";
import user2 from "../../assets/images/users/user2.jpg";
import user3 from "../../assets/images/users/user3.jpg";
import user4 from "../../assets/images/users/user4.jpg";
import user5 from "../../assets/images/users/user5.jpg";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrashAlt, faEdit } from '@fortawesome/free-solid-svg-icons';
import { Link } from "react-router-dom"
import axios from 'axios';
import React, { useEffect, useState } from 'react';
import {  useNavigate,useParams } from 'react-router-dom'

import { FilePond, registerPlugin } from 'react-filepond';
import 'filepond/dist/filepond.min.css';
import FilePondPluginImageExifOrientation from 'filepond-plugin-image-exif-orientation';
import FilePondPluginImagePreview from 'filepond-plugin-image-preview';
import 'filepond-plugin-image-preview/dist/filepond-plugin-image-preview.css';
import AccountEditModal from "./AccountEditModal";
import Swal from 'sweetalert2';
registerPlugin(FilePondPluginImageExifOrientation, FilePondPluginImagePreview);



const AccountsTables = ({accounts, account,setAccount, setAccounts}) => {
  const [isModalOpen, setIsModalOpen] = useState(false); // Pour contrôler l'ouverture du modal
  const [selectedcompte, setSelectecompte] = useState({
    id:" ",
    userName: " ",
  email: " ",
  password: "Abcde123456!",
  role: " ",
  avatar: " ",
  isActive: true,
  clienturi: "string"
  }); // Pour stocker le médicament 
  const [files, setFiles] = useState([]);

  const navigate=useNavigate()


  const fetchaccounts=async()=>{
    try {
      const res=await axios.get("http://localhost:5156/api/Account/GetAll")
      setAccounts(res.data)
      console.log(res.data)
     // setisLoading(false)
    } catch (error) {
      console.log(error)
    }

  }

  const serverOptions = () => {
    return {
      process: (fieldName, file, metadata, load, error, progress, abort) => {
        const data = new FormData();
        data.append('file', file);
        data.append('upload_preset', 'iit2025S1');
        data.append('cloud_name', 'esps');
        data.append('publicid', file.name);
        axios.post('https://api.cloudinary.com/v1_1/esps/image/upload', data)
          .then((response) => response.data)
          .then((data) => {
            setSelectecompte({ ...selectedcompte, avatar: data.url });
            load(data);
          })
          .catch((error) => {
            console.error('Error uploading file:', error);
            error('Upload failed');
            abort();
          });
      },
    };
  };


  const handleSave = async (e) => {
    e.preventDefault(); // Prevent form submission default action
    
   
  
    try {
      if (selectedcompte) {
        // Editing an existing medication
        // console.log("Updating:", selectedcompte.roles[0]);
      //   const accountData = {
      //     ...selectedcompte,
      //     password:selectedcompte.password,
      //     clienturi: " ",
      //     isActive: true
      // };

       console.log("update",selectedcompte)
        await axios.put(
          `http://localhost:5156/api/Account/Modify/${selectedcompte.id}`,
          selectedcompte
        );
      } 
  
      // Reset the form
      // setAccount({
      
      // });
      // setSelectecompte({})
      setFiles([]);
      setIsModalOpen(false); // Close modal
      // setSelectedMedicament(null); // Clear selected medication
       fetchaccounts(); // Refresh the list
      navigate("/dashboard/comptes"); // Redirect to the dashboard
    } catch (error) {
      console.error("Save operation failed:", error);
    }
  };

  const handleDelete = async (id) => {
    // Demander une confirmation avant la suppression
    if (window.confirm("Êtes-vous sûr de vouloir supprimer ce compte ?")) {
      try {
        // Envoyer la requête DELETE à l'API avec l'ID de l'article à supprimer
        await axios.delete(`http://localhost:5156/api/Account/Delete/${id}`);
  
        // Recharger la liste des médicaments après suppression
        fetchaccounts();
          Swal.fire('Supprimé !', 'Le compte a été supprimé.', 'success');
      } catch (error) {
        console.error("Erreur lors de la suppression du compte:", error);
        
      }
    }
  };


 const handleEdit = (med) => {
    console.log(med);
    // setSelectecompte(med); 
    // Access the roles array
const roles = med.roles;

// Access the first role
const firstRole = roles[0];
    
   
setSelectecompte({
  ...selectedcompte, // Spread the previous state
  id: med.id,
  role: firstRole,
  userName: med.userName,
  email: med.email,
  avatar: med.avatar,
});

    console.log("selectedCompte",selectedcompte);
    setIsModalOpen(true); // Open the modal
  // setAccount(med)

  setFiles(med.avatar)
  
      setFiles([{ source: med.image, options: { type: 'local' } }]); // Précharger l'image dans FilePond
      setIsModalOpen(true);
    };
    const styles = {
      formContainer: {
        display: 'flex',
        justifyContent: 'space-between',
        gap: '20px',
        width: '100%',
      },
      formGroup: {
        flex: 1,
      },
    };

  return (
    <div>
      <Card>
        <CardBody>
          {/* <CardTitle tag="h5">Project Listing</CardTitle> */}
          {/* <CardSubtitle className="mb-2 text-muted" tag="h6">
            Overview of the projects
          </CardSubtitle> */}

          <Table className="no-wrap mt-3 align-middle" responsive borderless>
            <thead>
              <tr>
                <th>Employés</th>
                <th>Role</th>

                {/* <th>Status</th>
                <th>Weeks</th> */}
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {accounts.map((tdata, index) => (
                <tr key={index} className="border-top">
                  <td>
                    <div className="d-flex align-items-center p-2">
                      <img
                        src={tdata.avatar}
                        className="rounded-circle"
                        alt="avatar"
                        width="45"
                        height="45"
                      />
                      <div className="ms-3">
                        <h6 className="mb-0">{tdata.userName}</h6>
                        <span className="text-muted">{tdata.email}</span>
                      </div>
                    </div>
                  </td>
                  <td>{tdata.roles[0]}</td>
             
                  <td>
                  <button className="btn btn-warning btn-sm me-2" onClick={() => handleEdit(tdata)}>
    <FontAwesomeIcon icon={faEdit} /> {/* Icon for Modify */}
    </button>
  <button className="btn btn-danger btn-sm"  onClick={() => handleDelete(tdata.id)} >
    <FontAwesomeIcon icon={faTrashAlt} /> {/* Icon for Delete */}
  </button>
</td>

                </tr>
              ))}
            </tbody>
          </Table>
        </CardBody>
      </Card>


      <AccountEditModal
        isModalOpen={isModalOpen}
        setIsModalOpen={setIsModalOpen}
        selectedCompte={selectedcompte}
        setSelectedCompte={setSelectecompte}
        handleSave={handleSave}
        files={files}
        setFiles={setFiles}
        serverOptions={serverOptions}
        styles={styles}
      />
      {/* Modal d'édition */}
      {/* <Modal isOpen={isModalOpen} toggle={() => setIsModalOpen(false)}>
        <ModalHeader toggle={() => setIsModalOpen(false)}>
          Modifier ce compte
        </ModalHeader>
        <ModalBody>
          <Form>
          <div style={styles.formContainer}>
          <FormGroup tyle={styles.formGroup}>
                <Label for="username">Username</Label>
                <Input
                  id="username"
                  // name="username"
                   value={selectedcompte.userName}
                  // onChange={(e)=>setSelectecompte({...selectedcompte,userName:e.target.value})}
                  placeholder="Enter your username"
                  type="text"
                />
              </FormGroup>

              <FormGroup style={styles.formGroup}>
                <Label for="email">Email</Label>
                <Input
                  id="email"
                  // name="email"
                  placeholder="Enter your email"
                  type="email"
                  value={selectedcompte.email}
                  // onChange={(e)=>setSelectecompte({...selectedcompte,email:e.target.value})}
                />
              </FormGroup>
              </div>
              <div style={styles.formContainer}>
              <FormGroup style={styles.formGroup}>
                <Label for="password">Old Password</Label>
                <Input
                  id="password"
                  // name="password"
                  placeholder="Enter your old password"
                  type="password"
                  // value={account.password}
                  // onChange={(e)=>setAccount({...account,password:e.target.value})}
                />
              </FormGroup>
              <FormGroup style={styles.formGroup}>
                <Label for="verifyPassword">New Password</Label>
                <Input
                  id="verifyPassword"
                  // name="verifyPassword"
                  placeholder="Enter your new password"
                  type="password"
                  // value={account.password_confirmation}
                  // onChange={(e)=>setAccount({...account,password_confirmation:e.target.value})}
                />
              </FormGroup>
              </div>
              <FormGroup>
  <Label for="role">Role</Label>
  <Input
    id="role"
    name="role"
    type="select"
    value={ selectedcompte.role }
    onChange={(e) => {
     
      // setSelectecompte({ ...selectedcompte, role: e.target.value }); // Update state immutably
    }}
  >
                    <option value="">Select a role</option>
                    <option value="medecin">Medecin</option>
                    <option value="patient">Patient</option>
                    <option value="pharmacien">Pharmacien</option>
                    <option value="Admin">Admin</option>
                    
                  </Input>
                </FormGroup>



                <FormGroup  >
                  <Label>Photo d'identité</Label>
                  <div style={{ width: "80%", margin: "auto", padding: "1%" }}>
                      <FilePond

                      files={files}
                      acceptedFileTypes="image/*"
                      onupdatefiles={setFiles}
                      allowMultiple={true}
                      server={serverOptions()}
                      name="file"

                      />
                      </div>
                </FormGroup>
          </Form>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={handleSave}>Enregistrer</Button> 
          <Button color="secondary" onClick={() => setIsModalOpen(false)}>Annuler</Button>
        </ModalFooter>
      </Modal> */}
    </div>
  );
};

export default AccountsTables;
