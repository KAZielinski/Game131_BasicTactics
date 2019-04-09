using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Actor))]
public class ActorEditor : Editor
{
    bool isBoardShowing = false;
    bool isActionEffectShowing = false;
    bool isImmunitiesShowing = false;
    

    public override void OnInspectorGUI()
    {
        Actor myActor = target as Actor;


        myActor.actorName = EditorGUILayout.TextField("Actor name", myActor.actorName);

        if(myActor.actorName == "")
        {
            myActor.actorName = myActor.name;
        }

        // max hit points
        myActor.maxHitPoints = EditorGUILayout.IntSlider("Max Hit Points", myActor.maxHitPoints, 0, 1000);

        // current hit points
        myActor.hitPoints = EditorGUILayout.IntSlider("Current Hit Points", myActor.hitPoints, 0, 1000);

        // Initiative
        myActor.initiative = EditorGUILayout.IntSlider("Initiative", myActor.initiative, 0, 50);

        // Damage
        myActor.damage = EditorGUILayout.IntSlider("Damage", myActor.damage, 0, 180);

        // Action target
        Actor.ActionTarget[] targetValue = Enum.GetValues(typeof(Actor.ActionTarget)) as Actor.ActionTarget[];
        string[] targetName = Enum.GetNames(typeof(Actor.ActionTarget));
        SelectionList<Actor.ActionTarget> targetSource = new SelectionList<Actor.ActionTarget>(targetValue, targetName);
        myActor.actionTarget = targetSource.RadioList("Action Target", myActor.actionTarget, 3);
        
        
        // Action Effect
        Actor.ActionEffect[] effectValue = Enum.GetValues(typeof(Actor.ActionEffect)) as Actor.ActionEffect[];
        string[] effectName = Enum.GetNames(typeof(Actor.ActionTarget));
        SelectionList<Actor.ActionEffect> actionEffect = new SelectionList<Actor.ActionEffect>(effectValue, effectName);
        myActor.actionEffect = actionEffect.RadioList("Action Effect", myActor.actionEffect, 3);

        // for action effect source
        Actor.ActionSource[] sourceValues = Enum.GetValues(typeof(Actor.ActionSource)) as Actor.ActionSource[];

        string[] sourceNames = Enum.GetNames(typeof(Actor.ActionSource));
        SelectionList<Actor.ActionSource> sources = new SelectionList<Actor.ActionSource>(sourceValues, sourceNames);

        isActionEffectShowing = EditorGUILayout.Foldout(isActionEffectShowing, "Action Effect Source");
        if (isActionEffectShowing)
        {
            myActor.actionEffectSource = sources.RadioList("Action Effect Source", myActor.actionEffectSource, 3);
        }
        // myActor.immunities = immune.SelectionList(myActor.source);

        // Immunities
        isImmunitiesShowing = EditorGUILayout.Foldout(isImmunitiesShowing, "Immunities");
        if (isImmunitiesShowing)
        {
            myActor.actionEffectSource = (Actor.ActionSource)EditorGUILayout.EnumPopup("Immunities", myActor.actionEffectSource);
        }

        // Percent Chance to hit
        myActor.percentChanceToHit = EditorGUILayout.Slider("Percent Chance", myActor.percentChanceToHit, 0.0f, 100.0f);

        // Board Position
        Actor.Position[] positionValue = Enum.GetValues(typeof(Actor.Position)) as Actor.Position[];
        string[] positionName = Enum.GetNames(typeof(Actor.Position));
        SelectionList<Actor.Position> position = new SelectionList<Actor.Position>(positionValue, positionName);
        
        isBoardShowing = EditorGUILayout.Foldout(isBoardShowing, "Board Position");
        if (isBoardShowing)
         {

                myActor.boardPosition = position.RadioList("Board Position", myActor.boardPosition, 3);

            
         }


    }

}


class SelectionList<T> where T : IComparable
{
    int f = 9;
    T[] _values;
    string[] _labels;
    T _selectedValue;


    public T[] CheckboxList(string label, T[] initialSelections, int itemsPerRow)
    {
        List<T> selectedValues = new List<T>();
        List<int> initialSelectedIndexes = new List<int>();
        for (int i = 0; i < _values.Length; i++)
        {
            for (int j = 0; j < initialSelections.Length; j++)
            {
                if (_values[i].CompareTo(initialSelections[j]) == 0) initialSelectedIndexes.Add(i);
            }
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.MaxWidth(100));

        EditorGUILayout.BeginVertical();
        for (int r = 0; r < _values.Length; r += itemsPerRow)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = r; i < r + itemsPerRow && i < _values.Length; i++)
            {
                if (GUILayout.Toggle(initialSelectedIndexes.Contains(i), _labels[i]))
                {
                    selectedValues.Add(_values[i]);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        return selectedValues.ToArray();

    }

    public T RadioList(string label, T initialSelection, int itemsPerRow)
    {
        T originalSelectedValue = _selectedValue;
        _selectedValue = initialSelection;
        bool anyChecked = false;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.MaxWidth(100));

        EditorGUILayout.BeginVertical();
        for (int r = 0; r < _values.Length; r += itemsPerRow)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = r; i < r + itemsPerRow && i < _values.Length; i++)
            {
                if (_values[i].CompareTo(initialSelection) == 0) originalSelectedValue = initialSelection;
                if (GUILayout.Toggle(_values[i].CompareTo(_selectedValue) == 0, _labels[i]))
                {
                    _selectedValue = _values[i];
                    anyChecked = true;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        if (!anyChecked) _selectedValue = originalSelectedValue;
        return _selectedValue;
    }

    public SelectionList(T[] values, string[] labels)
    {
        _values = new T[values.Length];
        _labels = new string[labels.Length < values.Length ? values.Length : labels.Length];
        for (int i = 0; i < _values.Length; i++) _values[i] = values[i];
        for (int i = 0; i < _labels.Length; i++) _labels[i] = (i < labels.Length) ? labels[i] : values[i].ToString();
        _selectedValue = _values[0];
    }
}