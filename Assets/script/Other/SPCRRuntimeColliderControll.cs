using System.Collections.Generic;
using UnityEngine;
using RootMotion;
using System;

public class SPCRRuntimeColliderControll 
{

    public SPCRRuntimeCollider[] bodyRuntimeCollider;

    public List<SPCRRuntimeCollider> otherRuntimeCollider;

    #region ColliderList  

    public enum ColliderBody
    {
        Hips =1,
        Spine=2,
        Head=3,
        leftUpperArm=4,
        leftLowerArm=5,
        rightUpperArm=6,
        rightLowerArm=7,
        leftUpperLeg=8,
        leftLowerLeg=9,
        rightUpperLeg=10,
        rightLowerLeg = 11,
        leftFoot=12,
        rightFoot=13,
        leftHand=14,
        rightHand=15,

    }

    SPCRRuntimeCollider headDynamicCollider;

    SPCRRuntimeCollider hipsDynamicCollider;

    SPCRRuntimeCollider spineDynamicCollider;

    SPCRRuntimeCollider leftUpperArmDynamicCollider;

    SPCRRuntimeCollider leftLowerArmDynamicCollider;

    SPCRRuntimeCollider rightUpperArmDynamicCollider;

    SPCRRuntimeCollider rightLowerArmDynamicCollider;

    SPCRRuntimeCollider leftUpperLegDynamicCollider;

    SPCRRuntimeCollider leftLowerLegDynamicCollider;

    SPCRRuntimeCollider rightUpperLegDynamicCollider;

    SPCRRuntimeCollider rightLowerLegDynamicCollider;

    SPCRRuntimeCollider[] leftHandDynamicColliders;

    SPCRRuntimeCollider[] rightHandDynamicColliders;

    SPCRRuntimeCollider leftFootDynamicCollider;

    SPCRRuntimeCollider rightFootDynamicCollider;
    #endregion

    #region Point and parameter
    public bool isGenerate;
    public float upperArmWidthAspect = 1f;
    public float lowerArmWidthAspect = 0.9f;
    public float endArmWidthAspect = 0.81f;

    public float upperLegWidthAspect = 1f;
    public float lowerLegWidthAspect = 0.7f;
    public float endLegWidthAspect = 0.7f;

    public Vector3 rootPoint;
    public Vector3 headStartPoint;
    public Vector3 headCenterPoint;
    public float headColliderRadiu;

    public Vector3 spineStopPoint;
    public Vector3 spineStartPoint;
    public float spineColliderRadiu;

    public Vector3 hipsStopPoint;
    public Vector3 hipsStartPoint;
    public float hipsColliderRadiu;

    public Vector3 upperArmToHeadCentroid;
    public Vector3 upperLegCentroid;

    public Vector3 leftHandCenterPoint;
    public Vector3 rightHandCenterPoint;
    public Vector3 leftFootCenterPoint;
    public Vector3 rightFootCenterPoint;

    public float torsoWidth;
    public float hipsWidth;
    public float headToRootHigh;
    #endregion
    public void GenerateCollidersData(GameObject character)
    {
        var animator = character.GetComponent<Animator>();
        if (animator == null || animator.avatar.isHuman)
        {
            isGenerate = false;
            return;
        }
        bodyRuntimeCollider = new SPCRRuntimeCollider[15];

        var head = animator.GetBoneTransform(HumanBodyBones.Head);
        var pelvis = animator.GetBoneTransform(HumanBodyBones.Hips);
        var spine = animator.GetBoneTransform(HumanBodyBones.Spine);

        var leftUpperLeg = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        var leftLowerLeg = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        var leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        var leftToes = animator.GetBoneTransform(HumanBodyBones.LeftToes);

        var rightUpperLeg = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        var rightLowerLeg = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
        var rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
        var rightToes = animator.GetBoneTransform(HumanBodyBones.RightToes);

        var leftUpperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        var leftLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        var leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
        var leftFinger = animator.GetBoneTransform(HumanBodyBones.LeftMiddleDistal);

        var rightUpperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
        var rightLowerArm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
        var rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
        var rightFinger = animator.GetBoneTransform(HumanBodyBones.RightMiddleDistal);

        rootPoint = character.transform.position;
        headStartPoint = head.position;

        upperArmToHeadCentroid =0.5f* (leftUpperArm.position+ rightUpperArm.position);
        torsoWidth = Vector3.Distance(leftUpperArm.position , rightUpperArm.position);

        upperLegCentroid = 0.5f * (leftUpperLeg.position + rightUpperLeg.position);
        hipsWidth = Vector3.Distance(leftUpperLeg.position , rightUpperLeg.position);

        //OYM：Head
        headCenterPoint = headStartPoint + new Vector3(0, 0.5f * torsoWidth, 0);
        headColliderRadiu = 0.5f * torsoWidth;
        bodyRuntimeCollider[0] = SphereCollider.GetSphereCollider(headColliderRadiu, headCenterPoint, head);


        // Spine
        spineStartPoint =spine.position;
        spineStopPoint = upperArmToHeadCentroid;
        spineColliderRadiu = torsoWidth * 0.5f;
        bodyRuntimeCollider[1] = CapsuleCollider.GetCapsuleCollider(spineColliderRadiu, spineColliderRadiu,spineStartPoint, spineStopPoint, spine);
        //Hip
        hipsStartPoint = upperLegCentroid  ;
        hipsStopPoint = spineStartPoint;
        hipsColliderRadiu = hipsWidth * 0.5f + Vector3.Distance(leftLowerLeg.position, leftUpperLeg.position) * 0.1f ;
        bodyRuntimeCollider[2] = CapsuleCollider.GetCapsuleCollider(spineColliderRadiu, hipsColliderRadiu, hipsStartPoint, hipsStopPoint, pelvis);

        // LeftArms

        float leftArmWidth = Vector3.Distance(leftUpperArm.position, leftLowerArm.position) *0.2f;
        float leftUpperArmWidth = leftArmWidth * upperArmWidthAspect;
        float leftLowerArmWidth = leftArmWidth * lowerArmWidthAspect;
        float leftEndArmWidth = leftArmWidth * endArmWidthAspect;

        bodyRuntimeCollider[3] = CapsuleCollider.GetCapsuleCollider(leftUpperArmWidth, leftLowerArmWidth, leftUpperArm.position, leftLowerArm.position,leftUpperArm);
        bodyRuntimeCollider[4] = CapsuleCollider.GetCapsuleCollider(leftLowerArmWidth, leftEndArmWidth, leftLowerArm.position, leftHand.position, leftLowerArm);


        // rightArms

        float rightArmWidth = Vector3.Distance(rightUpperArm.position, rightLowerArm.position) * 0.2f;
        float rightUpperArmWidth = rightArmWidth * upperArmWidthAspect;
        float rightLowerArmWidth = rightArmWidth * lowerArmWidthAspect;
        float rightEndArmWidth = rightArmWidth * endArmWidthAspect;

        bodyRuntimeCollider[5] = CapsuleCollider.GetCapsuleCollider(rightUpperArmWidth, rightLowerArmWidth, rightUpperArm.position, rightLowerArm.position, rightUpperArm);
        bodyRuntimeCollider[6] = CapsuleCollider.GetCapsuleCollider(rightLowerArmWidth, rightEndArmWidth, rightLowerArm.position, rightHand.position, rightLowerArm);

        // LeftLegs
        float leftLegWidth = Vector3.Distance(leftUpperLeg.position, leftLowerLeg.position);
        float leftUpperLegWidth = leftLegWidth * upperLegWidthAspect;
        float leftLowerLegWidth = leftLegWidth * lowerLegWidthAspect;
        float leftEndLegWidth = leftLegWidth * endLegWidthAspect;

        bodyRuntimeCollider[7] = CapsuleCollider.GetCapsuleCollider(leftUpperLegWidth, leftLowerLegWidth, leftUpperLeg.position, leftLowerLeg.position, leftUpperLeg);
        bodyRuntimeCollider[8] = CapsuleCollider.GetCapsuleCollider(leftLowerLegWidth, leftEndLegWidth, leftLowerLeg.position, leftHand.position, leftLowerLeg);

        // rightLegs
        float rightLegWidth = Vector3.Distance(rightUpperLeg.position, rightLowerLeg.position);
        float rightUpperLegWidth = rightLegWidth * upperLegWidthAspect;
        float rightLowerLegWidth = rightLegWidth * lowerLegWidthAspect;
        float rightEndLegWidth = rightLegWidth * endLegWidthAspect;

        bodyRuntimeCollider[9] = CapsuleCollider.GetCapsuleCollider(rightUpperLegWidth, rightLowerLegWidth, rightUpperLeg.position, rightLowerLeg.position, rightUpperLeg);
        bodyRuntimeCollider[10] = CapsuleCollider.GetCapsuleCollider(rightLowerLegWidth, rightEndLegWidth, rightLowerLeg.position, rightHand.position, rightLowerLeg);
        // LeftFoot

        if (leftToes != null)
        {
            bodyRuntimeCollider[11] = CapsuleCollider.GetCapsuleCollider(leftEndLegWidth, leftEndLegWidth, leftFoot.position, leftToes.position, leftFoot);

        }
        else
        {
            Vector3 leftfootStartPoint = leftFoot.position;
            Vector3 leftfootStopPoint = new Vector3(leftfootStartPoint.x, animator.rootPosition.y, leftfootStartPoint.z) + animator.rootRotation * Vector3.forward * -2 * leftLegWidth;
            bodyRuntimeCollider[11] = CapsuleCollider.GetCapsuleCollider(leftEndLegWidth, leftEndLegWidth, leftfootStartPoint, leftfootStopPoint, leftFoot);

        }

        // rightFoot

        if (rightToes != null)
        {
            bodyRuntimeCollider[12] = CapsuleCollider.GetCapsuleCollider(rightEndLegWidth, rightEndLegWidth, rightFoot.position, rightToes.position, rightFoot);

        }
        else
        {
            Vector3 rightfootStartPoint = rightFoot.position;
            Vector3 rightfootStopPoint = new Vector3(rightfootStartPoint.x, animator.rootPosition.y, rightfootStartPoint.z) + animator.rootRotation * Vector3.forward * -2 * rightLegWidth;
            bodyRuntimeCollider[12] = CapsuleCollider.GetCapsuleCollider(rightEndLegWidth, rightEndLegWidth, rightfootStartPoint, rightfootStopPoint, rightFoot);

        }

        // LeftHand

        var leftHandCenterPoint = (leftFinger.position + leftHand.position) * 0.5f;
        bodyRuntimeCollider[13] = SphereCollider.GetSphereCollider(Vector3.Distance(leftHand.position, leftHandCenterPoint), leftHandCenterPoint, leftHand);

        // rightHand

        var rightHandCenterPoint = (rightFinger.position + rightHand.position) * 0.5f;
        bodyRuntimeCollider[14] = SphereCollider.GetSphereCollider(Vector3.Distance(rightHand.position, rightHandCenterPoint), rightHandCenterPoint, rightHand);
    }

    public void OnDrawGizmos()
    {
        if (!isGenerate) return;

        for (int i = 0; i <= bodyRuntimeCollider?.Length - 1; i++)
        {
            bodyRuntimeCollider[i].OnDrawGizmos();
        }

    }

    
}
