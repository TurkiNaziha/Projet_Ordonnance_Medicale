import React from 'react'
import {Card, CardBody, FormGroup, CardTitle, Col, Row, Form, Label, Input, Button} from "reactstrap";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import  {useEffect, useState} from "react";
import {faClipboardList, faUserInjured, faUserMd} from "@fortawesome/free-solid-svg-icons";
import {FilePond} from "react-filepond";
import demande from "./Demande";
import axios from 'axios';

import 'filepond/dist/filepond.min.css';

import 'filepond-plugin-image-preview/dist/filepond-plugin-image-preview.css'
import ProjectTables from "../../components/dashboard/DemandesTables";



const Demande = () => {

    const[medicament, setMedicament] = useState({});
    const[patient, setPatient] = useState({});
    const[Demande, setDemande] = useState({
      
        id: 0,
        ordonnance: " ",
        patientName: " ",
        patientID: 0,
        medecinName: " ",
        medicaments: [
        ],
        etat: 0,
        date: "2024-12-14T17:48:38.815Z"
      }



    );
    const[Demandes,setDemandes] = useState([]);
    const [files, setFiles] = useState([]);
     const [patients, setPatients] = useState([]);
    

 const [currentUser, setCurrentUser] = useState({
    id:" ",
    userName: " ",
  email: " ",
  password: "Abcde123456!",
  role: " ",
  avatar: " ",
  isActive: true,
  clienturi: "string"
  }); // Pour stocker le médicament à modifier

 const fetchspatients=async()=>{
    try {
      const res=await axios.get("http://localhost:5156/api/Patient")
      setPatients(res.data )
      console.log("patients",res.data )
      // setisLoading(false)
    } catch (error) {
      console.log(error)
    }

  }


  const fetchUserData = async () => {
    const userData = localStorage.getItem("user");
    console.log("User data in localStorage:", userData);
    
    if (userData) {
      // Remove quotes around the string if present
      const parsedUserData = userData.replace(/"/g, ''); // This removes the double quotes
      
      try {
        // Pass username as part of the URL
        const res = await axios.get(`http://localhost:5156/api/Account/GetByUserName/${parsedUserData}`);
        // setCurrentUser(res.data);


        const roles = res.data.roles;

        // Access the first role
        const firstRole = roles[0];
            
           
        setCurrentUser({
          // Spread the previous state
          id: res.data.id,
          role: firstRole,
          userName: res.data.userName,
          email: res.data.email,
          avatar: res.data.avatar,
        });
          if(firstRole=="Patient")
          {
            // setDemande({...Demande,patientName:currentUser.userName})
          }
        console.log("Fetched user data:", currentUser);
        fetchDemandes(res.data.userName, firstRole)
      } catch (error) {
        console.error("Error fetching user data:", error);
      }
    } else {
      console.log("No user data found in localStorage.");
    }
  };


const fetchDemandes = async (usename, role) => {
  console.log("use",usename);
    try {
      if(role=="Medecin")
      {
        const res = await axios.get(`http://localhost:5156/api/Demand/by-medecin/${usename}`);
        setDemandes(res.data);
        console.log(res.data)
      }
      else if(role=="Patient") {
        const res = await axios.get(`http://localhost:5156/api/Demand/by-patient/${usename}`);
        setDemandes(res.data);
        console.log(res.data)
      }
      else{
        const res = await axios.get('http://localhost:5156/api/Demand');
        setDemandes(res.data);
        console.log(res.data)
      }
     
    } catch (error) {
      console.error('Error fetching demandes:', error);
    }
  };


  useEffect(() => {

    fetchUserData()
    fetchspatients()
    // fetchDemandes()
  
  }, []);

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
      
        setDemande({...Demande,ordonnance:data.url}) ;
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

      const handlePatientChange = (event) => {
        console.log("IDPat",event.target.value)
        // const selectedID = event.target.value; // Get selected medecin ID
        const p = patients.find((m) => m.id === Number(event.target.value));
 // Find the medecin object
        console.log("Pat",p)

        setDemande({
          ...Demande,
          patientID: event.target.value,
          patientName: p.nom,
          medecinName: p.medecinName,
      });
      
      };
 

 const handleSave = async (e) => {
    try {
      e.preventDefault();
  
      
      console.log("Données du demand :", Demande);

      await axios.post("http://localhost:5156/api/Demand", Demande)
          .then((res) => {
            setDemande(
              {
      
                id: 0,
                ordonnance: "",
                patientName: "",
                patientID: "",
                medecinName: "",
                medicaments: [
                  
                ],
                etat: 0,
                date: ""
              }
            
            );
            setFiles([])
            fetchDemandes()
            // setSelectedMedecin("");
            // fetch();

          });
    } catch (error) {
      console.error("Erreur lors de la creation d'un patient:", error);
    }
  };


  return (
    <div>
      <Row>
          <Col>
          {(currentUser.role === "Patient" || currentUser.role === "Admin") && (
    <Card>
        <CardTitle tag="h6" className="border-bottom p-3 mb-0">
            <FontAwesomeIcon icon={faClipboardList} style={{ marginRight: 20 }} />
            Gestion des Demandes
        </CardTitle>
        <CardBody>
            <Form>
                <div>
                    <FormGroup>
                        <Label>Date de creation</Label>
                        <Input
                            type="date"
                            name="date"
                            value={Demande.date}
                            onChange={(e) =>
                                setDemande({ ...Demande, date: e.target.value })
                            }
                        />
                    </FormGroup>
                    <FormGroup>
                        <Label>Ordonnance</Label>
                        <div style={{ width: "80%", margin: "auto", padding: "1%" }}>
                            <FilePond
                                files={files} // Ensure `files` contains the image
                                acceptedFileTypes="image/*"
                                onupdatefiles={setFiles}
                                allowMultiple={false} // Allow only one image
                                server={serverOptions()} // Set server options
                                name="file"
                            />
                        </div>
                    </FormGroup>
                    <FormGroup>
                        <Label for="patientID">Patient</Label>
                        <Input
                            id="patientID"
                            type="select"
                            name="patientID"
                            onChange={handlePatientChange} // Event handler for selection
                        >
                            <option value="">-- Sélectionner un Patient --</option>
                            {patients.map((p) => (
                                <option key={p.id} value={p.id}>
                                    {p.nom} {p.prenom}
                                </option>
                            ))}
                        </Input>
                    </FormGroup>
                </div>
                <Button onClick={(e) => handleSave(e)}>Ajouter</Button>
            </Form>
        </CardBody>
    </Card>
)}

          </Col>

          <Col lg="12">
        <ProjectTables  demandes={Demandes} demande={Demande} setDemandes={setDemandes} setDemande={setDemande} currentUser={currentUser}/>
        </Col>
      </Row>
    </div>
  )
}

export default Demande