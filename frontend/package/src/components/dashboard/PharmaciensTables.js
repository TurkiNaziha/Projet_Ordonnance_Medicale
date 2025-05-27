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
  registerPlugin(FilePondPluginImageExifOrientation, FilePondPluginImagePreview);
  
  
  
  const PharmaciensTables = ({pharmaciens, pharmacien, setPharmacien, setPharmaciens }) => {
    const [isModalOpen, setIsModalOpen] = useState(false); // Pour contrôler l'ouverture du modal
    const [selectedPharmacien, setSelectedPharmacien] = useState({
        nom: "",
        prenom: "",
        email: "",
    }); // Pour stocker le médicament 
  
    const navigate=useNavigate()
  
  
    const fetchaccounts=async()=>{
      try {
        const res=await axios.get("http://localhost:5156/api/Pharmacien")
        setPharmaciens(res.data)
        console.log(res.data)
       // setisLoading(false)
      } catch (error) {
        console.log(error)
      }
  
    }
  
  
  
  
    const handleSave = async (e) => {
      e.preventDefault(); // Prevent form submission default action
      
     
    
      try {
        if (selectedPharmacien) {
          // Editing an existing medication
          // console.log("Updating:", selectedcompte.roles[0]);
        //   const accountData = {
        //     ...selectedcompte,
        //     clienturi: " ",
        //     isActive: true
        // };
  
        // console.log("update",selectedPharmacien)
          await axios.put(
            `http://localhost:5156/api/Pharmacien/${selectedPharmacien.id}`,
            selectedPharmacien
          );
        } 
    
        // Reset the form
        // setAccount({
        
        // });
        setSelectedPharmacien({})
 
        setIsModalOpen(false); // Close modal
        // setSelectedMedicament(null); // Clear selected medication
         fetchaccounts(); // Refresh the list
        navigate("/dashboard/cards"); // Redirect to the dashboard
      } catch (error) {
        console.error("Save operation failed:", error);
      }
    };
  
    const handleDelete = async (id) => {
      // Demander une confirmation avant la suppression
      if (window.confirm("Êtes-vous sûr de vouloir supprimer ce pharamacien ?")) {
        try {
          // Envoyer la requête DELETE à l'API avec l'ID de l'article à supprimer
          await axios.delete(`http://localhost:5156/api/Pharmacien/${id}`);
    
          // Recharger la liste des médicaments après suppression
          fetchaccounts();
          navigate("/dashboard/cards");
        } catch (error) {
          console.error("Erreur lors de la suppression du pharamacien:", error);
        }
      }
    };
  
  
    const handleEdit = (med) => {
      console.log(med);
      setSelectedPharmacien(med); 
 
  
  // Access the first role
  
      // setMedicament(med); // Pre-fill the form with the selected medication details
      setIsModalOpen(true); // Open the modal
    // setAccount(med)
    console.log("a",med)
   
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
                  <th>Name</th>
                  {/* <th>LastName</th> */}
                  <th>Email</th>
                  {/* <th>Status</th>
                  <th>Weeks</th> */}
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {pharmaciens.map((tdata, index) => (
                  <tr key={index} className="border-top">
                    <td>
                      <div className="d-flex align-items-center p-2">
                        {/* <img
                          src={tdata.avatar}
                          className="rounded-circle"
                          alt="avatar"
                          width="45"
                          height="45"
                        /> */}
                        <div className="ms-3">
                          <h6 className="mb-0">{tdata.nom}</h6>
                          <span className="text-muted">{tdata.prenom}</span>
                        </div>
                      </div>
                    </td>
                    <td>{tdata.email}</td>
               
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
  
  
        
        {/* Modal d'édition */}
        <Modal isOpen={isModalOpen} toggle={() => setIsModalOpen(false)}>
          <ModalHeader toggle={() => setIsModalOpen(false)}>
            Modifier ce pharmacien
          </ModalHeader>
          <ModalBody>
            <Form>
            <div style={styles.formContainer}>
            <FormGroup style={styles.formGroup}>
  <Label for="username">Name</Label>
  <Input
    id="username"
    placeholder="Enter your username"
    type="text"
    value={selectedPharmacien.nom}
    onChange={(e) => 
      setSelectedPharmacien({
        ...selectedPharmacien,
        nom: e.target.value
      })
    }
  />
</FormGroup>
  
                

                    <FormGroup style={styles.formGroup}>
                        <Label for="prenom">First Name</Label>
                        <Input
                            id="prenom"
                            placeholder="Enter your first name"
                            type="text"
                            value={selectedPharmacien.prenom}
                            onChange={(e) => 
                            setSelectedPharmacien({
                                ...selectedPharmacien,
                                prenom: e.target.value
                            })
                            }
                        />
                        </FormGroup>


                </div>
                <div style={styles.formContainer}>
              
               <FormGroup style={styles.formGroup}>
                    <Label for="email">Email</Label>
                    <Input
                        id="email"
                        placeholder="Enter your email"
                        type="email"
                        value={selectedPharmacien.email}
                        onChange={(e) => 
                        setSelectedPharmacien({
                            ...selectedPharmacien,
                            email: e.target.value
                        })
                        }
                    />
                    </FormGroup>
                </div>
            
  
  
  
            
            </Form>
          </ModalBody>
          <ModalFooter>
            <Button color="primary" onClick={handleSave}>Enregistrer</Button> 
            <Button color="secondary" onClick={() => setIsModalOpen(false)}>Annuler</Button>
          </ModalFooter>
        </Modal>
      </div>
    );
  };
  
  export default PharmaciensTables;
  