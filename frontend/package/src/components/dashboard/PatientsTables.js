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



const PatientsTables = ({patients, patient, setPatient, setPatients }) => {
    const [isModalOpen, setIsModalOpen] = useState(false); // Pour contrôler l'ouverture du modal
    const [selectedPatient, setselectedPatient] = useState({
        nom: "",
        prenom: "",
        email: "",
    }); // Pour stocker le médicament

    const navigate=useNavigate()


    const fetchaccounts=async()=>{
        try {
            const res=await axios.get("http://localhost:5156/api/Patient")
            setPatients(res.data)
            console.log(res.data)
            // setisLoading(false)
        } catch (error) {
            console.log(error)
        }

    }




    const handleSave = async (e) => {
        e.preventDefault(); // Prevent form submission default action



        try {
            if (selectedPatient) {
                // Editing an existing medication
                // console.log("Updating:", selectedcompte.roles[0]);
                //   const accountData = {
                //     ...selectedcompte,
                //     clienturi: " ",
                //     isActive: true
                // };

                 console.log("update",selectedPatient)
                await axios.put(
                    `http://localhost:5156/api/Patient/${selectedPatient.id}` ,
                    selectedPatient
                );
            }

            // Reset the form
            // setAccount({

            // });
            setselectedPatient({})

            setIsModalOpen(false); // Close modal
            // setSelectedMedicament(null); // Clear selected medication
            fetchaccounts(); // Refresh the list
            navigate("/dashboard/buttons"); // Redirect to the dashboard
        } catch (error) {
            console.error("Save operation failed:", error);
        }
    };

    const handleDelete = async (id) => {
        // Demander une confirmation avant la suppression
        if (window.confirm("Êtes-vous sûr de vouloir supprimer ce patient ?")) {
            try {
                // Envoyer la requête DELETE à l'API avec l'ID de l'article à supprimer
                await axios.delete(`http://localhost:5156/api/Patient/${id}`);

                // Recharger la liste des médicaments après suppression
                fetchaccounts();
                navigate("/dashboard/buttons");
            } catch (error) {
                console.error("Erreur lors de la suppression du patient:", error);
            }
        }
    };


    const handleEdit = (med) => {
        console.log(med);
        setselectedPatient(med);


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
                            <th>Nom && Prenom</th>
                            {/* <th>LastName</th> */}
                            {/* <th>Prenom</th> */}
                            <th>Email</th>
                            <th>Telephone</th>
                            <th>Maladie</th>
                            <th>Medecin</th>
                            {/* <th>Status</th>
                  <th>Weeks</th> */}
                            <th>Actions</th>
                        </tr>
                        </thead>
                        <tbody>
                        {patients.map((tdata, index) => (
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
                                <td>{tdata.mail}</td>
                                <td>{tdata.telephone}</td>
                                <td>{tdata.maladie}</td>
                                <td>{tdata.medecinName}</td>

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
                    Modifier ce patient
                </ModalHeader>
                <ModalBody>
                    <Form>
                        <div style={styles.formContainer}>
                            <FormGroup style={styles.formGroup}>
                                <Label for="name">Name</Label>
                                <Input
                                    id="username"
                                    placeholder="Enter your last name"
                                    type="text"
                                    value={selectedPatient.nom}
                                    onChange={(e) =>
                                        setselectedPatient({...selectedPatient, nom: e.target.value})
                                    }
                                />
                            </FormGroup>


                            <FormGroup style={styles.formGroup}>
                                <Label for="prenom">First Name</Label>
                                <Input
                                    id="prenom"
                                    placeholder="Enter your first name"
                                    type="text"
                                    value={selectedPatient.prenom}
                                    onChange={(e) =>
                                        setselectedPatient({
                                            ...selectedPatient,
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
                                    value={selectedPatient.mail}
                                    onChange={(e) =>
                                        setselectedPatient({
                                            ...selectedPatient,
                                            mail: e.target.value
                                        })
                                    }
                                />
                            </FormGroup>
                            <FormGroup style={styles.formGroup}>
                                <Label for="tel">Telephone</Label>
                                <Input
                                    id="tel"
                                    placeholder="Enter your phone number"
                                    type="number"
                                    value={selectedPatient.telephone}
                                    onChange={(e) =>
                                        setselectedPatient({
                                            ...selectedPatient,
                                            telephone: e.target.value
                                        })
                                    }
                                />
                            </FormGroup>
                        </div>
                        <div style={styles.formContainer}>


                            <FormGroup style={styles.formGroup}>
                                <Label for="maladie">Maladie</Label>
                                <Input
                                    id="maladie"
                                    placeholder="Enter your illness"
                                    type="text"
                                    value={selectedPatient.maladie}
                                    onChange={(e) =>
                                        setselectedPatient({
                                            ...selectedPatient,
                                            maladie: e.target.value
                                        })
                                    }
                                />
                            </FormGroup>


                        </div>
                        <div style={styles.formContainer}/>


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

export default PatientsTables;